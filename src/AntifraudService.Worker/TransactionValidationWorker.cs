using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.DTOs;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Domain.Entities;
using System.Net.Http.Json;
using System.Text.Json;

namespace AntifraudService.Worker
{
    public class TransactionValidationWorker : BackgroundService
    {
        private readonly ILogger<TransactionValidationWorker> _logger;
        private readonly IMessageConsumer _messageConsumer;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _apiBaseUrl;

        public TransactionValidationWorker(
            ILogger<TransactionValidationWorker> logger,
            IMessageConsumer messageConsumer,
            HttpClient httpClient,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _messageConsumer = messageConsumer;
            _httpClient = httpClient;
            _serviceScopeFactory = serviceScopeFactory;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker para validar la transacción iniciando en: {time}", DateTimeOffset.Now);

            await _messageConsumer.ConsumeAsync(async (message) =>
            {
                await ProcessMessageAsync(message);
            }, stoppingToken);
        }

        private async Task ProcessMessageAsync(string messageJson)
        {
            try
            {
                var transactionMessage = JsonSerializer.Deserialize<TransactionMessage>(messageJson);

                if (transactionMessage == null)
                {
                    _logger.LogWarning("No se puede deserializar el mensaje");
                    return;
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var validationService = scope.ServiceProvider.GetRequiredService<TransactionValidationService>();
                    var transaction = new Transaction
                    {
                        Id = transactionMessage.TransactionExternalId,
                        SourceAccountId = transactionMessage.SourceAccountId,
                        Value = transactionMessage.Value,
                        CreatedAt = transactionMessage.CreatedAt
                    };

                    var validationResult = await validationService.ValidateTransaction(transaction);
                    _logger.LogInformation("Resultado de la validación de la transacción {id}: {result}",
                        transaction.Id, validationResult);

                    await UpdateTransactionStatusAsync(transaction.Id, validationResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando el mensaje");
            }
        }

        private async Task UpdateTransactionStatusAsync(Guid transactionId, TransactionStatus status)
        {
            try
            {
                var endpoint = $"{_apiBaseUrl}/api/Transactions/{transactionId}/status";
                var statusDto = new TransactionStatusDto
                {
                    TransactionExternalId = transactionId,
                    Status = status.ToString()
                };

                var response = await _httpClient.PutAsJsonAsync(endpoint, statusDto);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error actualizando el status de la transacción: {statusCode}", response.StatusCode);
                }
                else
                {
                    _logger.LogInformation("Transacción actualizada satisfactoriamente {id} status a {status}",
                        transactionId, status);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el estado de la transacción");
            }
        }
    }
}