using System.Threading.Tasks;

namespace AntifraudService.Application.Common.Interfaces
{
    public interface IMessageProducer
    {
        void Produce(string topic, string message);
        Task ProduceAsync(TransactionMessage transactionMessage);
    }
}