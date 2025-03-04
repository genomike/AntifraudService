using Microsoft.AspNetCore.Mvc;
using AntifraudService.Application.Features.Transactions.Commands.CreateTransaction;
using AntifraudService.Application.Features.Transactions.Queries.GetTransaction;
using AntifraudService.Application.DTOs;
using System.Threading.Tasks;
using MediatR;
using System;

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
            var command = new CreateTransactionCommand(transactionDto.SourceAccountId, transactionDto.TargetAccountId, transactionDto.TransferTypeId, transactionDto.Value);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTransaction), result);
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
    }
}