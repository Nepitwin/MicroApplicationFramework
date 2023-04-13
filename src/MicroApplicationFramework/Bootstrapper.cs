using MicroApplicationFramework.Interface;
using Serilog;

namespace MicroApplicationFramework;

public class Bootstrapper
{
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
            _application.OnRegister();
            _application.OnInit();
            _application.OnExecute();
            _application.OnExecuteAsync().Wait();
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
}