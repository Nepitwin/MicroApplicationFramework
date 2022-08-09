using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class App : Application
{
    public override void OnRegister()
    {
        Console.WriteLine("OnRegister");
        Container.Register<IModule, Module>(Reuse.Singleton);
        Container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit()
    {
        Console.WriteLine("OnInit");
    }

    public override void OnExecute()
    {
        Console.WriteLine("OnExecute");
        var module = Container.Resolve<IModule>();
        var moduleB = Container.Resolve<IModuleB>();

        // Write your logic code here ...
        module.Foo();
        moduleB.Bar();
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
    }
}