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

        public abstract void OnRegister();
        public abstract void OnInit();
        public abstract void OnExecute();
        public abstract void OnExit();
    }
}