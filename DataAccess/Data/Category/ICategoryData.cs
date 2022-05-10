using DataAccess.Models;

namespace DataAccess.Data.Category
{
    public interface ICategoryData
    {
        Task DeleteCategory(int idCategory);
        Task<CategoryDbModel> GetCategory(int idCategory);
        Task<IEnumerable<CategoryDbModel>> GetCategorys();
        Task InserCategory(CategoryDbModel category);
    }
}