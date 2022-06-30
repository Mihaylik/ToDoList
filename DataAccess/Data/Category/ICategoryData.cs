using DataAccess.Models;
using SqlDataAccess.Data;

namespace DataAccess.Data.Task
{
    public interface ICategoryData : IData
    {
        System.Threading.Tasks.Task DeleteCategory(int idCategory);
        Task<CategoryDbModel> GetCategory(int idCategory);
        Task<IEnumerable<CategoryDbModel>> GetCategories();
        System.Threading.Tasks.Task InserCategory(CategoryDbModel category);
    }
}