using DryIoc;
using MicroApplicationFramework;

namespace MicroApplicationFrameworkExample;

public class App2: Application
{
    public override void OnExecute(IContainer container)
    {
        Console.WriteLine("OnExecute");
        base.OnExecute(container);
    }
}