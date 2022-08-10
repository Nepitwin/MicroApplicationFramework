using System.Collections.Concurrent;
using MicroApplicationFramework.Interface;

namespace MicroApplicationFramework;

public class ApplicationTaskScheduler : ITaskScheduler
{
    private static readonly Mutex Mutex = new();
    private readonly ConcurrentBag<Task> _tasksCollection = new();

    public void Clear()
    {
        Mutex.WaitOne();
        _tasksCollection.Clear();
        Mutex.ReleaseMutex();
    }

    public Task[] GetScheduledTasks()
    {
        Mutex.WaitOne();
        var array = _tasksCollection.ToArray();
        Mutex.ReleaseMutex();
        return array;
    }

    public void Add(Task task)
    {
        Mutex.WaitOne();
        _tasksCollection.Add(task);
        Mutex.ReleaseMutex();
    }
}