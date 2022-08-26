# Micro Application Framework (MAF)

MAF is a container based library to quickly build small service applications.

* MAF uses the DryIoc container as a dependency injection framework.
* MAF guarantees a simple lifecycle to manage all modules.

## Lifecycle

Executed lifecycle from a application are:

1. OnRegister() - Registration from all all modules.
2. OnInit()     - Module initialization.
3. OnExecute()  - Execution method which guarantees an pre initialization and registration.
4. OnExit()     - Exit request to dispose or cleanup all modules.

## Interface

* IApplicationContext - AplicationContext
  * API-Interface to handle application context usages for example to request a cancel from the application.
    * RequestOnCancel
    * Timeout -> Default Values is 5 Minutes to call RequestOnCancel

### Example

Just implement the lifecycle and launch the application using the bootstrapper.

```
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
```

### Code execution

Execute application by bootstraper

```
Bootstrapper.Create(new App()).Run();
```

### Cancelation event from IApplicationContext

In each cycle you have access to the IApplicationContext, which you can use to cancel all tasks at any time.

```
public override void OnExecute(IContainer container)
{
    ApplicationContext.RequestCancel();
}
```

The application context also contains a timeout parameter that can be used to control how long an application may be active until it is terminated. 

The default value for this is 5 minutes.

```
public override void OnInit(IContainer container)
{
    // For example application timeout is called by 500 ms.
    ApplicationContext.Timeout = 500;
}
```
