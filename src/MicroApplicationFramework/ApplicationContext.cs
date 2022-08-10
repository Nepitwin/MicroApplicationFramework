using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationContext : IApplicationContext
{
    public ITaskScheduler TaskScheduler { get; } = new ApplicationTaskScheduler();

    public event IApplicationContext.CancelEventHandler CancelEvent = null!;

    public void RequestCancel()
    {
        CancelEvent.Invoke();
    }
}