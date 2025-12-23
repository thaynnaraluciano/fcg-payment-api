using Api.Utils;
using CrossCutting.Exceptions.Middlewares;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;
using Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos;
using Domain.Commands.v1.Pagamentos.CancelarPagamento;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using Domain.MapperProfiles;
using FluentValidation;
using Infrastructure.Data.Context;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

#region MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CriarPagamentoCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CancelarPagamentoCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarTodosPagamentosCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarPagamentoPorIdCommandHandler).Assembly));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BuscarPagamentoPorUsuarioCommandHandler).Assembly));
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
#endregion

#region Interfaces
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
#endregion

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connString,
        new MySqlServerVersion(new Version(8, 0, 42))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<MiddlewareTratamentoDeExcecoes>();

app.Run();
