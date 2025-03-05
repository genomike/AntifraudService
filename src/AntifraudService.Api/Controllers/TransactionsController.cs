using AntifraudService.Application.DTOs;
using AntifraudService.Application.Features.Transactions.Commands.CreateTransaction;
using AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using AntifraudService.Application.Features.Transactions.Queries.GetTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AntifraudService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
        {
            var command = new CreateTransactionCommand(
                transactionDto.SourceAccountId, 
                transactionDto.TargetAccountId, 
                transactionDto.TransferTypeId, 
                transactionDto.Value);
            
            var result = await _mediator.Send(command);
            
            return CreatedAtAction(
                nameof(GetTransaction),
                new { id = result },
                new { id = result }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var query = new GetTransactionQuery(id);
            var transaction = await _mediator.Send(query);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTransactionStatus(Guid id, [FromBody] TransactionStatusDto statusDto)
        {
            statusDto.TransactionExternalId = id;
            
            var command = new UpdateTransactionStatusCommand(
                statusDto.TransactionExternalId,
                statusDto.Status);
            
            var result = await _mediator.Send(command);
            
            if (!result)
                return NotFound($"Transacción con ID {id} no encontrada");
            
            return NoContent();
        }
    }
}