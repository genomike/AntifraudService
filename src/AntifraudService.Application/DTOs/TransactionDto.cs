using System;

namespace AntifraudService.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}