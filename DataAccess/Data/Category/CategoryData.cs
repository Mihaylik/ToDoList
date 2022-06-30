using Dapper;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Data.Task
{
    public class CategoryData : ICategoryData
    {
        private readonly IConfiguration _config;

        public CategoryData(IConfiguration config)
        {
            _config = config;
        }
        
        public async Task<IEnumerable<CategoryDbModel>> GetCategories() {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            return await connection.QueryAsync<CategoryDbModel>("select * from Category");
        }
        public async Task<CategoryDbModel> GetCategory(int idCategory)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            var allAnswers = await connection.QueryAsync<CategoryDbModel>("select * from Category where idCategory = " + idCategory);
            return allAnswers.First();
        }
        public async System.Threading.Tasks.Task InserCategory(CategoryDbModel category)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            string sqlquery = @"insert into Category(name) values (@name)";
            await connection.ExecuteAsync(sqlquery, category);
        }

        public async System.Threading.Tasks.Task DeleteCategory(int idCategory)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.QueryAsync($"delete from Category where idCategory = {idCategory}");
        }
    }
}
