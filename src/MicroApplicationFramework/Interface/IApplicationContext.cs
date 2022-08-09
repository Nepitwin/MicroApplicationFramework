using System.Collections.Concurrent;

namespace MicroApplicationFramework.Interface;

public interface IApplicationContext
{
    ConcurrentBag<Task> Tasks { get; }

    delegate void CancelEventHandler();
    event CancelEventHandler CancelEvent;
    void RequestCancel();
}