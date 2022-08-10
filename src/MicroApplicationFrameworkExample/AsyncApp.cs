﻿using DryIoc;
using MicroApplicationFramework;
using MicroApplicationFrameworkExample.Interface;
using MicroApplicationFrameworkExample.Service;

namespace MicroApplicationFrameworkExample;

public class AsyncApp: Application
{
    public override void OnRegister()
    {
        Console.WriteLine("OnRegister");
        Container.Register<IModule, Module>(Reuse.Singleton);
        Container.Register<IModuleB, ModuleB>(Reuse.Singleton);
    }

    public override void OnInit()
    {
        Console.WriteLine("OnInit");
    }

    public override void OnExecute()
    {
        Console.WriteLine("OnExecute");

        ApplicationContext.TaskScheduler.Add(Task.Run(async () =>
        {
            await Task.Delay(10000);
            Console.WriteLine("Delayed Task Foo");
            var module = Container.Resolve<IModule>();
            module.Foo();

            ApplicationContext.TaskScheduler.Add(Task.Run(() =>
            {
                Console.WriteLine("Extend additional tasks if needed.");
            }));
        }));

        ApplicationContext.TaskScheduler.Add(Task.Run(async () =>
        {
            await Task.Delay(8000);
            Console.WriteLine("Delayed Task Bar");
            var moduleB = Container.Resolve<IModuleB>();
            moduleB.Bar();
        }));
    }

    public override void OnExit()
    {
        Console.WriteLine("OnExit");
    }
}