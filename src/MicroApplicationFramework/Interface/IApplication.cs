namespace MicroApplicationFramework.Interface;

public interface IApplication
{
    public void OnRegister();
    public void OnInit();
    public void OnExecute();
    public Task OnExecuteAsync();
    public void OnExit();

    public void NotifyOnRegisterFinished();
}