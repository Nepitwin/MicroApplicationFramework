namespace MicroApplicationFramework.Interface;

public interface ITaskScheduler
{ 
    public void Produce(Task task);
    
    internal Task[] ConsumeAllTasks();
}