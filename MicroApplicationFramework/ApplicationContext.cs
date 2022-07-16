using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationContext : IApplicationContext
{
    public event IApplicationContext.CancelEventHandler? CancelEvent;

    public void RequestCancel()
    {
        CancelEvent?.Invoke();
    }
}