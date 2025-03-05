using System;
using System.Threading;
using System.Threading.Tasks;

namespace AntifraudService.Application.Common.Interfaces
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync(Func<string, Task> messageHandler, CancellationToken cancellationToken);
    }
}