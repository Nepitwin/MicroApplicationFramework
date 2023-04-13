using MicroApplicationFramework;
using MicroApplicationFrameworkExample;

Console.WriteLine("----------------");
Console.WriteLine("Sync App");
Bootstrapper.Create(new App()).Run();
Console.WriteLine("----------------");

Console.WriteLine("----------------");
Console.WriteLine("Async App");
Bootstrapper.Create(new AsyncApp()).Run();
Console.WriteLine("----------------");
