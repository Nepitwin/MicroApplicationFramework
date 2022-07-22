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
    }

    public void Run()
    {
        _application.OnRegister(_container);
        _application.OnInit(_container);
        InitContext();
        _container.Resolve<IApplicationContext>();
        _application.OnExecute();
        while (!_cancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(1000);
        }
        _application.OnExit();
    }
    private void InitContext()
    {
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