using System;
using System.Text.Json.Serialization;

namespace AntifraudService.Application.DTOs
{
    public class TransactionDto
    {
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
    }
}