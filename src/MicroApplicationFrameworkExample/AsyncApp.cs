using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class AsyncApp: Application
{
    public override void OnRegister()
    {
        base.OnRegister();
        Console.WriteLine("OnRegister");
        Container.Register<IModule, Module>(Reuse.Singleton);
        Container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit()
    {
        base.OnInit();
        Console.WriteLine("OnInit");
    }

    public override Task OnExecuteAsync()
    {
        Console.WriteLine("OnExecuteAsync");
        return Task.Run(async () =>
        {
            var module = Container.Resolve<IModule>();
            var moduleB = Container.Resolve<IModuleB>();

            // Write your async logic code here ...
            module.Foo();
            await Task.Delay(2500);
            moduleB.Bar();

            // OnExit will be called if all task is finished
        });
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
        base.OnExit();
    }
}