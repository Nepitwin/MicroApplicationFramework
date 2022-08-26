using MicroApplicationFramework.Interface;
using Serilog;

namespace MicroApplicationFramework;

public class Bootstrapper
{
    private readonly CancellationTokenSource _cts = new();

    private readonly IApplication _application;

    public static Bootstrapper Create(IApplication application)
    {
        return new Bootstrapper(application);
    }

    private Bootstrapper(IApplication application)
    {
        _application = application;
    }

    public void Run()
    {
        try
        {
            InitContext();
            _application.OnRegister();
            _application.OnInit();
            _application.OnExecute();
            _cts.Token.WaitHandle.WaitOne(_application.ApplicationContext.Timeout);
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                Log.Warning("Timeout reached...application will be canceled");
            }
        }
        catch (Exception ex)
        { 
            Log.Warning(ex,"Exception called");
        }
        finally
        {
            _application.OnExit();
        }
    }

    private void InitContext()
    {
        _application.ApplicationContext.CancelEvent += OnCancelRequested;
    }

    private void OnCancelRequested()
    {
        _cts.Cancel();
    }
}