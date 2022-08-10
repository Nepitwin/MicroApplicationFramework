using MicroApplicationFramework;
using MicroApplicationFrameworkExample;

Console.WriteLine("----------------");
Bootstrapper.Create(new App()).Run();
Console.WriteLine("----------------");
Bootstrapper.Create(new AsyncApp()).Run();
Console.WriteLine("----------------");
