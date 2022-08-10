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

## Interfaces

* IApplicationContext - AplicationContext
  * API-Interface to handle application context usages for example to request a cancel from the application.
  * Stores all Tasks which should be running async to the application

## Examples

Two examples are implemented for a synchronized and async execution handling.

### Synchronized

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
```

### Async

If you want to run multiple tasks in the background you can access them via the ApplicationContext and register individual tasks seperatly.
The application waits until all tasks are completed or the cancel event has been called.

```
public class AsyncApp: Application
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

        ApplicationContext.TaskScheduler.Add(Task.Run(async () =>
        {
            await Task.Delay(10000);
            Console.WriteLine("Delayed Task Foo");
            var module = Container.Resolve<IModule>();
            module.Foo();

            ApplicationContext.TaskScheduler.Add(Task.Run(() =>
            {
                Console.WriteLine("Extend additional tasks if needed.");
            }));
        }));

        ApplicationContext.TaskScheduler.Add(Task.Run(async () =>
        {
            await Task.Delay(8000);
            Console.WriteLine("Delayed Task Bar");
            var moduleB = Container.Resolve<IModuleB>();
            moduleB.Bar();
        }));
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
    }
}
```

#### Cancel event from IApplicationContext

In each cycle you have access to the IApplicationContext, which you can use to cancel all tasks at any time.

```
public override void OnExecute(IContainer container)
{
    ApplicationContext.RequestCancel();
}
```

## Code execution

Execute application by bootstraper

```
Bootstrapper.Create(new App()).Run();
```
