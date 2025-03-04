using System;

public class TransactionMessage
{
    public Guid TransactionExternalId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TargetAccountId { get; set; }
    public int TransferTypeId { get; set; }
    public decimal Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
}