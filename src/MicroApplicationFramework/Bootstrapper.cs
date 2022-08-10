using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class Bootstrapper
{
    private CancellationTokenSource _cts = new();

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

            var tasks = _application.ApplicationContext.TaskScheduler.GetScheduledTasks();

            while (tasks.Length > 0)
            {
                _cts = new CancellationTokenSource();
                _application.ApplicationContext.TaskScheduler.Clear();
                Task.WaitAll(tasks, _cts.Token);
                tasks = _application.ApplicationContext.TaskScheduler.GetScheduledTasks();
            }
        }
        catch (Exception)
        { 
            // Ignored
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