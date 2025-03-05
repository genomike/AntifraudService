using System;

namespace AntifraudService.Application.DTOs
{
    public class TransactionStatusDto
    {
        public Guid TransactionExternalId { get; set; }
        public string Status { get; set; }
    }
}