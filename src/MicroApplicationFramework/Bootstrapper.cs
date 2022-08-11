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
            var tasks = _application.ApplicationContext.TaskCollector.ConsumeAllTasks();
            while (tasks.Length > 0)
            {
                Task.WaitAll(tasks, _cts.Token);
                tasks = _application.ApplicationContext.TaskCollector.ConsumeAllTasks();
            }
        }
        catch (AggregateException aggregateException)
        {
            Log.Warning("The following exceptions have been thrown");
            foreach (var ex in aggregateException.InnerExceptions)
            {
                Log.Warning(ex, ex.ToString());
            }
        }
        catch (OperationCanceledException ex)
        {
            Log.Information(ex,"Operation was cancelled by application");
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
        if (_cts.IsCancellationRequested)
        {
            return;
        }

        _cts.Cancel();
    }
}