using System.Collections.Concurrent;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationContext : IApplicationContext
{
    public ConcurrentBag<Task> Tasks { get; } = new();

    public event IApplicationContext.CancelEventHandler CancelEvent = null!;

    public void RequestCancel()
    {
        CancelEvent.Invoke();
    }
}