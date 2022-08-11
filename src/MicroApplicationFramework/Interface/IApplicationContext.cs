namespace MicroApplicationFramework.Interface;

public interface IApplicationContext
{
    ITaskCollector TaskCollector { get; }

    delegate void CancelEventHandler();
    event CancelEventHandler CancelEvent;

    void RequestCancel();
}