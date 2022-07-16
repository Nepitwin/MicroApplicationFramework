# Micro Application Framework (MAF)

MAF is a container based library to quickly build small service applications.

MAF uses the DryIoc container as a dependency injection framework.

## Lifecycle

Executed lifecycle from a application are:

1. OnRegister(Icontainer container) - Register all your Interfaces
2. OnInit(Icontainer container)     - Initialize all modules if needed
3. OnExecute() - Execution method which guarantees all initializatíon and registration from modules are finished.
4. OnExit() - Exit requested to dispose or cleanup all services if needed

## Interfaces

* IApplicationContext
  * API-Interface to handle application context usages for example to request a cancel from the application. 

## Example

An example is implemented under MicroApplicationFrameworkExample project.

```
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
```

Execute application by bootstrape
```
using MicroApplicationFramework;
using MicroApplicationFrameworkExample;

var bootstrapper = new Bootstrapper(new App());
bootstrapper.Run();
```
