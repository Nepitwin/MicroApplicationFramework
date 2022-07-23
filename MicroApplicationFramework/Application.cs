using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework
{
    public abstract class Application : IApplication
    {
        protected IApplicationContext ApplicationContext = null!;

        public virtual void OnRegister(IContainer container)
        {
            container.Register<IApplicationContext, ApplicationContext>(Reuse.Singleton);
        }

        public virtual void OnInit(IContainer container)
        {
            ApplicationContext = container.Resolve<IApplicationContext>();
        }

        public virtual void OnExecute(IContainer container)
        {
            ApplicationContext.RequestCancel();
        }

        public virtual void OnExit(IContainer container)
        {
        }
    }
}