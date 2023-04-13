# Micro Application Framework (MAF)

MAF is a container based library to quickly build small service applications.

* MAF uses the DryIoc container as a dependency injection framework.
* MAF guarantees a simple lifecycle management.

## Lifecycle

Executed lifecycle from a application are:

1. OnRegister()     - Registration from all all modules.
2. OnInit()         - Module initialization.
3. OnExecute()      - Execution method which guarantees an pre initialization and registration from dependencies.
4. OnExecuteAsync() - Async execution method which guarantees an pre initialization and registration from dependencies.
5. OnExit()         - Exit request to dispose or cleanup all modules.

### Example

Just implement the lifecycle and launch the application using from bootstrapper.

#### Sync

```
public class App : Application
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
        base.OnExit();
    }
}
```

#### Async

```
public class App : Application
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
```

### Application run

Execute application from bootstraper

```
Bootstrapper.Create(new App()).Run();
```
