using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationContext : IApplicationContext
{
    public ITaskCollector TaskCollector { get; } = new TaskCollector();

    public event IApplicationContext.CancelEventHandler CancelEvent = null!;

    public void RequestCancel()
    {
        CancelEvent.Invoke();
    }
}