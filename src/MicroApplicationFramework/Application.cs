using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework
{
    public abstract class Application : IApplication
    {
        public IContainer Container { get; }

        protected Application()
        {
            Container = new Container();
        }

        public virtual void OnRegister()
        {
            // Ignored
        }

        public virtual void OnInit()
        {
            // Ignored
        }

        public virtual void OnExecute()
        {
            // Ignored
        }

        public virtual Task OnExecuteAsync()
        {
            // Ignored
            return Task.Run(() => {});
        }

        public virtual void OnExit()
        {
            // Ignored
        }
    }
}