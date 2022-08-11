namespace MicroApplicationFramework.Interface;

public interface ITaskCollector
{ 
    public void Produce(Task task);
    
    internal Task[] ConsumeAllTasks();
}