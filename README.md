# Micro Application Framework (MAF)

MAF is a container based library to quickly build small service applications.

* MAF uses the DryIoc container as a dependency injection framework.
* MAF guarantees a simple lifecycle to manage all modules.

## Lifecycle

Executed lifecycle from a application are:

1. OnRegister(IContainer container) - Registration from all all modules.
2. OnInit(IContainer container)     - Module initialization.
3. OnExecute(IContainer container) - Execution method which guarantees an pre initialization and registration.
4. OnExit(IContainer container) - Exit request to dispose or cleanup all modules.

## Interfaces

* IApplicationContext - AplicationContext
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
```

## Cancel event for application

Application execution will not be automatically shutdown if OnExecute is exited for async tasks executions.
Developers can decide to shutdown application by a simple RequestCancel execution from ApplicationContext

```
public override void OnExecute(IContainer container)
{
    // Logic code here
    //...
    //...
    // Call event to shutdown your application
    ApplicationContext.RequestCancel();
}
```

If no async tasks are needed simple call base.OnExecute() for an ApplicationContext.RequestCancel().

```
public override void OnExecute(IContainer container)
{
    Console.WriteLine("OnExecute");
    base.OnExecute();
}
```

If nothing will be called application will at the state never shutdown.

## Code execution


Execute application by bootstraper
```
using MicroApplicationFramework;
using MicroApplicationFrameworkExample;

var bootstrapper = new Bootstrapper(new App());
bootstrapper.Run();
```
