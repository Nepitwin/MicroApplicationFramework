using System.Collections.Concurrent;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationTaskScheduler : ITaskScheduler
{
    private static readonly Mutex Mutex = new();
    private readonly List<Task> _tasksCollection = new();
    
    Task[] ITaskScheduler.ConsumeAllTasks()
    {
        Mutex.WaitOne();
        var array = _tasksCollection.ToArray();
        _tasksCollection.Clear();
        Mutex.ReleaseMutex();
        return array;
    }
    
    public void Produce(Task task)
    {
        Mutex.WaitOne();
        _tasksCollection.Add(task);
        Mutex.ReleaseMutex();
    }
}