using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class App : Application
{
    private IModule _module = null!;
    private IModuleB _moduleB = null!;

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
        _module = container.Resolve<IModule>();
        _moduleB = container.Resolve<IModuleB>();
    }

    public override void OnExecute(IContainer container)
    {
        Console.WriteLine("OnExecute");

        // Write your logic code here ...
        _module.Foo();
        _moduleB.Bar();

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