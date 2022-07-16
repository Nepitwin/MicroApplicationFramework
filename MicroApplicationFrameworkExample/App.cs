using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFramework.Interface;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class App : Application
{
    private IModule? _module;
    private IModuleB? _moduleB;
    private IApplicationContext? _applicationContext;

    public override void OnRegister(IContainer container)
    {
        Console.WriteLine("OnRegister");
        container.Register<IModule, Module>(Reuse.Singleton);
        container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit(IContainer container)
    {
        Console.WriteLine("OnInit");
        _module = container.Resolve<IModule>();
        _moduleB = container.Resolve<IModuleB>();
        _applicationContext = container.Resolve<IApplicationContext>();
    }

    public override void OnExecute()
    {
        Console.WriteLine("OnExecute");

        // Write your logic code here ...
        _module?.Foo();
        _moduleB?.Bar();

        // Application will not be automatically shutdown if OnExecute will be exited for async tasks executions
        // Developer can decide to shutdown application by execution request cancel method from application context
        _applicationContext?.RequestCancel();
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
    }
}