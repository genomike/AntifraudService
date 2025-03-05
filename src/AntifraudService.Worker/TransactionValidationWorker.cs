using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.DTOs;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            _logger.LogInformation("Transaction Validation Worker starting at: {time}", DateTimeOffset.Now);

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
                    _logger.LogWarning("Could not deserialize message");
                    return;
                }

                // Create a scope to resolve scoped services
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // Resolve the scoped service within the scope
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var validationService = scope.ServiceProvider.GetRequiredService<TransactionValidationService>();

                    // Get transaction from database
                    var transaction = await transactionRepository.GetTransactionById(transactionMessage.TransactionExternalId);
                    
                    if (transaction == null)
                    {
                        _logger.LogWarning("Transaction not found: {id}", transactionMessage.TransactionExternalId);
                        return;
                    }

                    // Validate transaction
                    var validationResult = await validationService.ValidateTransaction(transaction);
                    _logger.LogInformation("Transaction {id} validation result: {result}", 
                        transaction.Id, validationResult);

                    // Update transaction status via API
                    await UpdateTransactionStatusAsync(transaction.Id, validationResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
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
                    _logger.LogError("Failed to update transaction status: {statusCode}", response.StatusCode);
                }
                else
                {
                    _logger.LogInformation("Successfully updated transaction {id} status to {status}", 
                        transactionId, status);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction status");
            }
        }
    }
}