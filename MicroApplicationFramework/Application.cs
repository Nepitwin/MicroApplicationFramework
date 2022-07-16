using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework
{
    public abstract class Application : IApplication
    {
        public abstract void OnRegister(IContainer container);
        public abstract void OnInit(IContainer container);
        public abstract void OnExecute();
        public abstract void OnExit();
    }
}