using MicroApplicationFramework.Interface;
using MicroApplicationFrameworkExample.Interface;

namespace MicroApplicationFrameworkExample.Service;

public class Module : IModule
{
    private IApplicationContext _context;

    public Module(IApplicationContext context)
    {
        Console.WriteLine("Module INIT");
        _context = context;
    }

    public void Foo()
    {
        Console.WriteLine("Foo");
    }
}