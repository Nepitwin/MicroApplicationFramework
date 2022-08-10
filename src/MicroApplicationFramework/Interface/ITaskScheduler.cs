namespace MicroApplicationFramework.Interface;

public interface ITaskScheduler
{
    public void Clear();
    public Task[] GetScheduledTasks();
    public void Add(Task task);
}