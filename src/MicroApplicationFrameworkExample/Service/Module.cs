using MicroApplicationFramework.Interface;
using MicroApplicationFrameworkExample.Interface;

namespace MicroApplicationFrameworkExample.Service;

public class Module : IModule
{
    public Module()
    {
        Console.WriteLine("Module INIT");
    }

    public void Foo()
    {
        Console.WriteLine("Foo");
    }
}