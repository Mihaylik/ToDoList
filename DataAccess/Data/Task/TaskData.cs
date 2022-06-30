using Dapper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DataAccess.Models;

namespace DataAccess.Data
{
    public class TaskData : ITaskData
    {
        private readonly IConfiguration _config;

        public TaskData(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<TaskDbModel>> GetTasks()
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.QueryAsync<TaskDbModel>("select * from TaskInfo");

        }
        public async Task<TaskDbModel> GetTask(int idTask)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            var allAnswers = await connection.QueryAsync<TaskDbModel>("select * from TaskInfo where idTask = " + idTask);
            return allAnswers.First();
        }
        public async System.Threading.Tasks.Task InsertTask(TaskDbModel task)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            string sqlquery = @"insert into TaskInfo(name, timeStart, deadline, passed, idCategory) values (@name, @timeStart, @deadline, @passed, @idCategory)";
            await connection.ExecuteAsync(sqlquery, task);
        }

        public async System.Threading.Tasks.Task UpdateTask(TaskDbModel task)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            string sqlquery = @"update TaskInfo set name = @name, timeStart = @timeStart, deadline = @deadline, passed = @passed, idCategory = @idCategory where idTask = @idTask";
            await connection.QueryAsync(sqlquery, task);
        }

        public async System.Threading.Tasks.Task DeleteTask(int idTask)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.QueryAsync($"delete from TaskInfo where TaskInfo.idTask = {idTask}");
        }
    }
}
