using MicroApplicationFramework;

namespace MicroApplicationFrameworkExample;

public class App2: Application
{
    public override void OnExecute()
    {
        Console.WriteLine("OnExecute");
        base.OnExecute();
    }
}