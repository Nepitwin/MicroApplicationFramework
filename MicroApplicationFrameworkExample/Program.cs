using MicroApplicationFramework;
using MicroApplicationFrameworkExample;

Console.WriteLine("----- First Application Start -----");
var bootstrapper = new Bootstrapper(new App());
bootstrapper.Run();
Console.WriteLine("----- First Application Stop -----");

Console.WriteLine("----- Second Application Start -----");
var bootstrapper2 = new Bootstrapper(new App2());
bootstrapper2.Run();
Console.WriteLine("----- Second Application Stop -----");
