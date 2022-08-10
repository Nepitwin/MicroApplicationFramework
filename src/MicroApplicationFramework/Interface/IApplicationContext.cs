namespace MicroApplicationFramework.Interface;

public interface IApplicationContext
{
    ITaskScheduler TaskScheduler { get; }

    delegate void CancelEventHandler();
    event CancelEventHandler CancelEvent;

    void RequestCancel();
}