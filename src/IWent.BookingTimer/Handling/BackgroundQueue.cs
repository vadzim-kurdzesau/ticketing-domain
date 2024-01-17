using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace IWent.BookingTimer.Handling;

public class BackgroundQueue<T> : IBackgroundQueue<T>
{
    private readonly Channel<T> _queue;

    public BackgroundQueue()
    {
        _queue = Channel.CreateUnbounded<T>();
    }

    public ValueTask EnqueueAsync(T item, CancellationToken cancellationToken)
    {
        return _queue.Writer.WriteAsync(item, cancellationToken);
    }

    public ValueTask<T> DequeueAsync(CancellationToken cancellationToken)
    {
        return _queue.Reader.ReadAsync(cancellationToken);
    }
}
