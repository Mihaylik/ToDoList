using DataAccess.Models;
using SqlDataAccess.Data;

namespace DataAccess.Data
{
    public interface ITaskData : IData
    {
        System.Threading.Tasks.Task DeleteTask(int idTask);
        Task<TaskDbModel> GetTask(int idTask);
        Task<IEnumerable<TaskDbModel>> GetTasks();
        System.Threading.Tasks.Task InsertTask(TaskDbModel task);
        System.Threading.Tasks.Task UpdateTask(TaskDbModel task);
    }
}