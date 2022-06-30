namespace ToDoListSANA.GraphVariables.Task
{
    public class TaskEdit : InputObjectGraphType
    {
        public TaskEdit()
        {
            Name = "TaskEdit";
            Field<IntGraphType>("idTask");
            Field<StringGraphType>("name");
            Field<DateTimeGraphType>("timeStart");
            Field<DateTimeGraphType>("deadline");
            Field<NonNullGraphType<BooleanGraphType>>("passed");
            Field<NonNullGraphType<IntGraphType>>("idCategory");
        }
    }
}
