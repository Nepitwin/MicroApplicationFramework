using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;
using Serilog;

namespace MicroApplicationFrameworkExample;

public class App : Application
{
    public override void OnRegister()
    {
        Log.Information("OnRegister");
        Container.Register<IModule, Module>(Reuse.Singleton);
        Container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit()
    {
        Log.Information("OnInit");
    }

    public override void OnExecute()
    {
        Log.Information("OnExecute");
        var module = Container.Resolve<IModule>();
        var moduleB = Container.Resolve<IModuleB>();

        // Write your logic code here ...
        Log.Information(module.Foo());
        Log.Information(moduleB.Bar());
    }

    public override void OnExit()
    {
        Log.Information("OnExit");
    }
}