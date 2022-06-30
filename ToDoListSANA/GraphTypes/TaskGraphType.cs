using DataAccess.Models;
using ToDoListSANA.Models;

namespace ToDoListSANA.GraphTypes
{
    public class TaskGraphType : ObjectGraphType<TaskDbModel>
    {
        public TaskGraphType()
        {
            Field(x => x.idTask);
            Field(x => x.name, nullable: true);
            Field(x => x.timeStart, nullable: true);
            Field(x => x.deadline, nullable: true);
            Field(x => x.passed);
            Field(x => x.idCategory);
        }
    }
}
