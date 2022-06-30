namespace ToDoListSANA.GraphVariables.Categories
{
    public class CategoryEdit : InputObjectGraphType
    {
        public CategoryEdit()
        {
            Name = "CategoryEdit";
            Field<IntGraphType>("idCategory");
            Field<StringGraphType>("name");
        }
    }
}
