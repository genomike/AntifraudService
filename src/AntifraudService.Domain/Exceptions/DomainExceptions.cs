using System;

namespace AntifraudService.Domain.Exceptions
{
    public class TransactionValidationException : Exception
    {
        public TransactionValidationException(string message) : base(message)
        {
        }

        public TransactionValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}