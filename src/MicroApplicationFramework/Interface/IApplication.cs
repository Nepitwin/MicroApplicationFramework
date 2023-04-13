using DryIoc;

namespace MicroApplicationFramework.Interface;

public interface IApplication
{
    public IContainer Container { get; }
    public IApplicationContext ApplicationContext { get; }
    public void OnRegister();
    public void OnInit();
    public void OnExecute();
    public Task OnExecuteAsync();
    public void OnExit();
}