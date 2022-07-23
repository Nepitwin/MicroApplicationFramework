using DryIoc;

namespace MicroApplicationFramework.Interface;

public interface IApplication
{
    public void OnRegister(IContainer container);
    public void OnInit(IContainer container);
    public void OnExecute(IContainer container);
    public void OnExit(IContainer container);
}