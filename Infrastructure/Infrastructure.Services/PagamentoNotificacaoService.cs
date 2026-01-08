using Amazon.Lambda;
using Amazon.Lambda.Model;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Environment = System.Environment;

namespace Infrastructure.Services;

public class PagamentoNotificacaoService : IPagamentoNotificacaoService
{
    private readonly IAmazonLambda _lambda;
    private readonly ILogger<PagamentoNotificacaoService> _logger;
    private readonly string _nomeFuncaoLambda;

    public PagamentoNotificacaoService(
            IAmazonLambda lambda,
            ILogger<PagamentoNotificacaoService> logger)
    {
        _lambda = lambda;
        _logger = logger;

        // variável de ambiente do container (ECS)
        _nomeFuncaoLambda = Environment.GetEnvironmentVariable("PAYMENT_NOTIFICATION_LAMBDA")
                           ?? throw new InvalidOperationException("Variável de ambiente PAYMENT_NOTIFICATION_LAMBDA não foi definida.");
    }

    public async Task NotificarAsync(Guid pagamentoId, Guid userId, decimal valor, StatusPagamento status)
    {
        var payload = new NotificacaoPagamentoPayload
        {
            PagamentoId = pagamentoId,
            UserId = userId,
            Valor = valor,
            Status = (StatusPagamento)(int)status
        };

        var json = JsonSerializer.Serialize(payload);

        _logger.LogInformation("Invocando Lambda {Lambda} com payload: {Payload}", _nomeFuncaoLambda, json);

        var request = new InvokeRequest
        {
            FunctionName = _nomeFuncaoLambda,
            InvocationType = InvocationType.Event, // assíncrono (não trava seu request)
            Payload = json
        };

        var response = await _lambda.InvokeAsync(request);

        // Para InvocationType.Event, o "StatusCode" esperado costuma ser 202
        if (response.StatusCode < 200 || response.StatusCode >= 300)
        {
            var responsePayload = response.Payload != null ? await new StreamReader(response.Payload).ReadToEndAsync() : "";
            _logger.LogError("Falha ao invocar Lambda. StatusCode={StatusCode} FunctionError={FunctionError} Payload={ResponsePayload}",
                response.StatusCode, response.FunctionError, responsePayload);

            throw new Exception($"Erro ao invocar Lambda de notificação. StatusCode={response.StatusCode}");
        }

        _logger.LogInformation("Lambda invocada com sucesso. StatusCode={StatusCode}", response.StatusCode);
    }

    private class NotificacaoPagamentoPayload
    {
        public Guid PagamentoId { get; set; }
        public Guid UserId { get; set; }
        public decimal Valor { get; set; }
        public StatusPagamento Status { get; set; }
    }
}
