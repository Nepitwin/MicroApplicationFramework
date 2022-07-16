namespace MicroApplicationFramework.Interface;

public interface IApplicationContext
{
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    public void RequestCancel();
}