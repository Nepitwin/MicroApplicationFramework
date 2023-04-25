using MicroApplicationFrameworkExample.Interface;
using Serilog;

namespace MicroApplicationFrameworkExample.Service;

public class ModuleB : IModuleB
{
    private readonly IModule _module;

    public ModuleB(IModule module)
    {
        Log.Information("ModuleB INIT");
        _module = module;
    }

    public string Bar()
    {
        return _module.Foo() + " Bar";
    }
}