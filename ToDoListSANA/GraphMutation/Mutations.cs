using DataAccess.Data.Task;
using DataAccess.Models;
using GraphQL;
using ToDoListSANA.Enums;
using ToDoListSANA.GraphTypes;
using ToDoListSANA.GraphVariables.Categories;
using ToDoListSANA.GraphVariables.Task;
using ToDoListSANA.Switcher;

namespace ToDoListSANA.GraphMutation
{
    public class Mutations : ObjectGraphType
    {
        private readonly ITaskData taskData;
        private readonly ICategoryData categoryData;
        public Mutations(IEnumerable<ITaskData> taskDataResolve, IEnumerable<ICategoryData> categoryDataResolve, IHttpContextAccessor contextAccessor)
        {
            DataProvider dataProvider;
            Enum.TryParse(contextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            this.taskData = taskDataResolve.GetPropered(dataProvider);
            this.categoryData = categoryDataResolve.GetPropered(dataProvider);
            Field<TaskGraphType>("createTask",
                arguments: new QueryArguments { new QueryArgument<TaskInput> { Name = "task" } },
                resolve: x =>
                {
                    taskData.InsertTask(x.GetArgument<TaskDbModel>("task"));
                    return null;
                });
            Field<TaskGraphType>("deleteTask",
                arguments: new QueryArguments(new QueryArgument<IntGraphType>() { Name = "idTask" }),
                resolve: x =>
                {
                    var returned = taskData.GetTask(x.GetArgument<int>("idTask"));
                    taskData.DeleteTask(x.GetArgument<int>("idTask"));
                    return returned;
                });
            Field<TaskGraphType>("updateTask",
                arguments: new QueryArguments { new QueryArgument<TaskEdit> { Name = "task" } },
                resolve: x =>
                {
                    taskData.UpdateTask(x.GetArgument<TaskDbModel>("task"));
                    return x.GetArgument<TaskDbModel>("task");
                });
            Field<CategoryGraphType>("createCategory",
                arguments: new QueryArguments { new QueryArgument<CategoryInput> { Name = "category" } },
                resolve: x =>
                {
                    categoryData.InserCategory(x.GetArgument<CategoryDbModel>("category"));
                    return null;
                });
            Field<CategoryGraphType>("deleteCategory",
                arguments: new QueryArguments { new QueryArgument<IntGraphType> { Name = "idCategory" } },
                resolve: x =>
                {
                    var returned = categoryData.GetCategory(x.GetArgument<int>("idCategory")).Result;
                    categoryData.DeleteCategory(x.GetArgument<int>("idCategory"));
                    return returned;
                });

        }
    }
}
