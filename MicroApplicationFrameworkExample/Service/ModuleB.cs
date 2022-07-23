using MicroApplicationFrameworkExample.Interface;

namespace MicroApplicationFrameworkExample.Service;

public class ModuleB : IModuleB
{
    private readonly IModule _module;

    public ModuleB(IModule module)
    {
        Console.WriteLine("ModuleB INIT");
        _module = module;
    }

    public void Bar()
    {
        _module.Foo();
        Console.WriteLine("Bar");
    }

    public void Init()
    {
        Console.WriteLine("Init");
    }
}