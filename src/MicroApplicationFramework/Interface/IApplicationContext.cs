namespace MicroApplicationFramework.Interface;

public interface IApplicationContext
{
    delegate void CancelEventHandler();
    event CancelEventHandler CancelEvent;
    int Timeout { get; set; }
    void RequestCancel();
}