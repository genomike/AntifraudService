using System.Threading.Tasks;

namespace AntifraudService.Application.Common.Interfaces
{
    public interface IMessageProducer
    {
        Task Produce(TransactionMessage transactionMessage);
    }
}