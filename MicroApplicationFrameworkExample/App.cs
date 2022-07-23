using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class App : Application
{
    public override void OnRegister(IContainer container)
    {
        base.OnRegister(container);
        Console.WriteLine("OnRegister");
        container.Register<IModule, Module>(Reuse.Singleton);
        container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit(IContainer container)
    {
        base.OnInit(container);
        Console.WriteLine("OnInit");
        // If a init method is needed
        container.Resolve<IModule>().Init();
        container.Resolve<IModuleB>().Init();
    }

    public override void OnExecute(IContainer container)
    {
        Console.WriteLine("OnExecute");

        // By default all registered object dependency will be resolved by all registered object by on register
        // Inversion of control handles object management
        var module = container.Resolve<IModule>();
        var moduleB = container.Resolve<IModuleB>();

        // Write your logic code here ...
        module.Foo();
        moduleB.Bar();

        // Application will not be automatically shutdown if OnExecute will be exited for async tasks executions
        // Developer can decide to shutdown application by execution request cancel method from Application context...
        Task.Run(async delegate
        {
            await Task.Delay(Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds));
            ApplicationContext.RequestCancel();
        });
    }

    public override void OnExit(IContainer container)
    {
        Console.WriteLine("OnExit");
        base.OnExit(container);
    }
}