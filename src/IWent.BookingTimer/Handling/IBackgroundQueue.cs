using System.Threading;
using System.Threading.Tasks;

namespace IWent.BookingTimer.Handling;
public interface IBackgroundQueue<T>
{
    ValueTask EnqueueAsync(T item, CancellationToken cancellationToken);

    ValueTask<T> DequeueAsync(CancellationToken cancellationToken);
}