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
        try
        {
            Console.WriteLine("OnExecute");
            var module = Container.Resolve<IModule>();
            var moduleB = Container.Resolve<IModuleB>();

            // Write your logic code here ...
            module.Foo();
            moduleB.Bar();
        }
        finally
        {
            // Application should be canceled if on execute is finished
            // If method will not be called a default timeout by 5 minutes will stop the application afterwards.
            ApplicationContext.RequestCancel();
        }
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
    }
}