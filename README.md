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
```

#### Async

```
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
```

### Application run

Execute application from bootstraper

```
Bootstrapper.Create(new App()).Run();
```

### How to mock tests

Simply override the corresponding event after registering the services and overwrite it with your own mock implementations.

```
        [Fact] 
        public void ContainerElementCanBeOverrideToMockResults()
        {
            using (InitLoggerContext())
            {
                var app = new App();

                app.OnRegisterFinishedEventHandler += container =>
                {
                    // Replace service results by mocking
                    var mockModule = new Mock<IModule>();
                    mockModule.Setup(mock => mock.Foo()).Returns("mockbar");
                    var mockBModule = new Mock<IModuleB>();
                    mockBModule.Setup(mock => mock.Bar()).Returns("mockfoo");

                    container.RegisterInstance(mockModule.Object, IfAlreadyRegistered.Replace);
                    container.RegisterInstance(mockBModule.Object, IfAlreadyRegistered.Replace);
                };

                Bootstrapper.Create(app).Run();

                var logs = TestCorrelator.GetLogEventsFromCurrentContext().ToArray();
                logs.Should().Contain(log => log.MessageTemplate.Text == "mockbar");
                logs.Should().Contain(log => log.MessageTemplate.Text == "mockfoo");
            }
        }
```
