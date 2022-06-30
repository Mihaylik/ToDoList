namespace ToDoListSANA.GraphVariables.Task
{
    public class TaskInput : InputObjectGraphType
    {
        public TaskInput()
        {
            Name = "TaskInput";
            Field<StringGraphType>("name");
            Field<DateTimeGraphType>("timeStart");
            Field<DateTimeGraphType>("deadline");
            Field<NonNullGraphType<BooleanGraphType>>("passed");
            Field<NonNullGraphType<IntGraphType>>("idCategory");
        }
    }
}
