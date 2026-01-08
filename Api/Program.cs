using Amazon.Lambda;
using Api.Extensions;
using Api.Utils;
using CrossCutting.Exceptions.Middlewares;
using CrossCutting.Monitoring;
using Domain;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;
using Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos;
using Domain.Commands.v1.Pagamentos.CancelarPagamento;
using Domain.Commands.v1.Pagamentos.ConfirmarPagamento;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using Domain.Interfaces;
using Domain.MapperProfiles;
using FluentValidation;
using Infrastructure.Data.Context;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using Prometheus.DotNetRuntime;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 🔗 HTTP Client para UserAPI
builder.Services.AddHttpClient("UserApi", client =>
{
    var baseUrl = builder.Configuration["Services:UserApi"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new Exception("Services:UserApi não configurado.");

    client.BaseAddress = new Uri(baseUrl);
});

// 🎮 HTTP Client para GameAPI
builder.Services.AddHttpClient("GameApi", client =>
{
    var baseUrl = builder.Configuration["Services:GameApi"];

    if (string.IsNullOrWhiteSpace(baseUrl))
        throw new Exception("Services:GameApi não configurado.");

    client.BaseAddress = new Uri(baseUrl);
});


builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FIAP Cloud Games - Payment API",
        Version = "v1",
        Description = @"
            FIAP Cloud Games é uma plataforma de venda de jogos digitais e gestão de servidores para partidas online.

            - Pagamentos: Buscar pagamentos, cancelar pagamento, criar e confirmar pagamento."
    });
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

#region MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CriarPagamentoCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CancelarPagamentoCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarTodosPagamentosCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarPagamentoPorIdCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarPagamentoPorUsuarioCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ConfirmarPagamentoCommandHandler).Assembly));
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(PagamentoProfile));
#endregion

#region Validators
builder.Services.AddScoped<IValidator<CriarPagamentoCommand>, CriarPagamentoCommandValidator>();
builder.Services.AddScoped<IValidator<CancelarPagamentoCommand>, CancelarPagamentoCommandValidator>();
builder.Services.AddScoped<IValidator<BuscarTodosPagamentosCommand>, BuscarTodosPagamentosCommandValidator>();
builder.Services.AddScoped<IValidator<BuscarPagamentoPorUsuarioCommand>, BuscarPagamentoPorUsuarioCommandValidator>();
builder.Services.AddScoped<IValidator<BuscarPagamentoPorIdCommand>, BuscarPagamentoPorIdCommandValidator>();
builder.Services.AddScoped<IValidator<ConfirmarPagamentoCommand>, ConfirmarPagamentoCommandValidator>();
#endregion

#region Interfaces
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IPagamentoNotificacaoService, PagamentoNotificacaoService>();
#endregion

builder.Services.AddAWSService<IAmazonLambda>(); // usa credenciais do ambiente (ECS Task Role)
builder.Services.AddSingleton<IMetricsService, MetricsService>();
builder.Services.AddScoped<ValidacaoServicosExternos>();

// Le variáveis de ambiente (do SO, .env ou secrets)
string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string db = Environment.GetEnvironmentVariable("DB_NAME") ?? "Bd_Payments";
string user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
string pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root";

string connString = $"Server={host};Database={db};User={user};Password={pass};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connString,
        new MySqlServerVersion(new Version(8, 0, 43))
    ));

var app = builder.Build();

// Para rodar com API Gateway na AWS, usar caminho base /payment
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        swaggerDoc.Servers = new List<OpenApiServer>
        {
            new() { Url = "/payment" }
        };
    });
});

app.UseSwaggerUI();

app.UseHttpsRedirection();

DotNetRuntimeStatsBuilder.Default().StartCollecting();

app.UseRouting();

app.UseRequestMetrics();
app.MapMetrics();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/health", () => Results.Ok("Healthy"));

app.UseMiddleware<MiddlewareTratamentoDeExcecoes>();

app.Run();
