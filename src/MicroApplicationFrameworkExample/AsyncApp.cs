using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;
using Serilog;

namespace MicroApplicationFrameworkExample;

public class AsyncApp: Application
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

    public override Task OnExecuteAsync()
    {
        Log.Information("OnExecuteAsync");
        return Task.Run(async () =>
        {
            var module = Container.Resolve<IModule>();
            var moduleB = Container.Resolve<IModuleB>();

            // Write your async logic code here ...
            Log.Information(module.Foo());
            await Task.Delay(2500);
            Log.Information(moduleB.Bar());

            // OnExit will be called if all task is finished
        });
    }

    public override void OnExit()
    {
        Log.Information("OnExit");
    }
}