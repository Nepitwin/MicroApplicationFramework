using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework
{
    public abstract class Application : IApplication
    {
        public IContainer Container { get; }

        public event OnRegisterFinished? OnRegisterFinishedEventHandler;
        public delegate void OnRegisterFinished(IContainer container);

        protected Application()
        {
            Container = new Container();
        }

        public void NotifyOnRegisterFinished()
        {
            OnRegisterFinishedEventHandler?.Invoke(Container);
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
            return Task.Run(() =>
            {
                // Ignored
            });
        }

        public virtual void OnExit()
        {
            // Ignored
        }
    }
}