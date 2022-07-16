using DryIoc;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class Bootstrapper
{
    private readonly IContainer _container = new Container();

    private readonly IApplication _application;

    private readonly CancellationTokenSource _cancellationToken = new();

    public Bootstrapper(IApplication application)
    {
        _application = application;
        InitContext();
    }

    public void Run()
    {
        _application.OnRegister(_container);
        _application.OnInit(_container);
        _application.OnExecute();
        while (!_cancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(1000);
        }
        _application.OnExit();
    }
    private void InitContext()
    {
        _container.Register<IApplicationContext, ApplicationContext>(Reuse.Singleton);
        var context = _container.Resolve<IApplicationContext>();
        context.CancelEvent += OnCancelRequested;
    }

    private void OnCancelRequested()
    {
        if (_cancellationToken.IsCancellationRequested)
        {
            return;
        }

        _cancellationToken.Cancel();
    }
}