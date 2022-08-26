using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationContext : IApplicationContext
{
    public event IApplicationContext.CancelEventHandler CancelEvent = null!;
    public int Timeout { get; set; } = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;

    public void RequestCancel()
    {
        CancelEvent.Invoke();
    }
}