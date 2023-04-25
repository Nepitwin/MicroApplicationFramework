using MicroApplicationFrameworkExample.Interface;
using Serilog;

namespace MicroApplicationFrameworkExample.Service;

public class Module : IModule
{
    public Module()
    {
        Log.Information("Module INIT");
    }

    public string Foo()
    {
        return "Foo";
    }
}