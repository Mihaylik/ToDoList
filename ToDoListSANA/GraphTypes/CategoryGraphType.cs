using DataAccess.Models;

namespace ToDoListSANA.GraphTypes
{
    public class CategoryGraphType : ObjectGraphType<CategoryDbModel>
    {
        public CategoryGraphType()
        {
            Field(x => x.idCategory);
            Field(x => x.name);
        }
    }
}
