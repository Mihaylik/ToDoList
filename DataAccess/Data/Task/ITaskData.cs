using DataAccess.Models;

namespace DataAccess.Data
{
    public interface ITaskData
    {
        Task DeleteTask(int idTask);
        Task<TaskDbModel> GetTask(int idTask);
        Task<IEnumerable<TaskDbModel>> GetTasks();
        Task InserTask(TaskDbModel task);
        Task UpdateTask(TaskDbModel task);
    }
}