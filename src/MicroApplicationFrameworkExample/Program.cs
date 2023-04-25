using MicroApplicationFramework;
using MicroApplicationFrameworkExample;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("----------------");
Log.Information("Sync App");
Bootstrapper.Create(new App()).Run();
Log.Information("----------------");

Log.Information("----------------");
Log.Information("Async App");
Bootstrapper.Create(new AsyncApp()).Run();
Log.Information("----------------");
