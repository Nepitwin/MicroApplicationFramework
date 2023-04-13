using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework
{
    public abstract class Application : IApplication
    {
        public IContainer Container { get; }
        public IApplicationContext ApplicationContext { get; }

        protected Application()
        {
            Container = new Container();
            Container.Register<IApplicationContext, ApplicationContext>(Reuse.Singleton);
            ApplicationContext = Container.Resolve<IApplicationContext>();
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