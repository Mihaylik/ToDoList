namespace ToDoListSANA.GraphVariables.Categories
{
    public class CategoryInput : InputObjectGraphType
    {
        public CategoryInput()
        {
            Name = "CategoryInput";
            Field<StringGraphType>("name");
        }
    }
}
