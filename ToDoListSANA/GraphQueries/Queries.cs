using DataAccess.Data.Task;
using GraphQL;
using ToDoListSANA.Enums;
using ToDoListSANA.GraphTypes;
using ToDoListSANA.Switcher;

namespace ToDoListSANA.GraphQueries
{
    public class Queries : ObjectGraphType
    {
        private readonly ITaskData taskData;
        private readonly ICategoryData categoryData;
        public Queries(IEnumerable<ITaskData> taskDataResolve, IEnumerable<ICategoryData> categoryDataResolve, IHttpContextAccessor contextAccessor)
        {

            DataProvider dataProvider;
            Enum.TryParse(contextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            this.taskData = taskDataResolve.GetPropered(dataProvider);
            this.categoryData = categoryDataResolve.GetPropered(dataProvider);
            Field<ListGraphType<TaskGraphType>>(Name = "tasks", resolve: x => taskData.GetTasks().Result);
            Field<ListGraphType<CategoryGraphType>>(Name = "categories", resolve: x => categoryData.GetCategories().Result);
            Field<TaskGraphType>(Name = "task",
                                 arguments: new QueryArguments(new QueryArgument<IntGraphType>() { Name = "idTask" }),
                                 resolve: x => taskData.GetTask(x.GetArgument<int>("idTask")).Result);
            Field<CategoryGraphType>(Name = "category",
                                     arguments: new QueryArguments(new QueryArgument<IntGraphType>() { Name = "idCategory" }),
                                     resolve: x => categoryData.GetCategory(x.GetArgument<int>("idCategory")).Result);
            Name = "Queries";
        }
    }
}
