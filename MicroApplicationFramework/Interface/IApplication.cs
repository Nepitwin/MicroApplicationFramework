using DryIoc;

namespace MicroApplicationFramework.Interface;

public interface IApplication
{
    public void OnRegister(IContainer container);
    public void OnInit(IContainer container);
    public void OnExecute();
    public void OnExit();
}