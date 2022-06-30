global using DataAccess.Data;
global using GraphQL.Types;
using DataAccess.Data.Task;
using XmlDataAccess.Repositories;
using ToDoListSANA.GraphSchemes;
using GraphQL.Server;
using ToDoListSANA.GraphTypes;
using ToDoListSANA.GraphQueries;
using ToDoListSANA.GraphMutation;
using ToDoListSANA.GraphVariables.Task;
using ToDoListSANA.GraphVariables.Categories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
// Add DB signatures
builder.Services.AddSingleton<ITaskData, TaskData>();
builder.Services.AddSingleton<ICategoryData, CategoryData>();
builder.Services.AddSingleton<ITaskData, XmlTaskRepository>();
builder.Services.AddSingleton<ICategoryData, XmlCategoryRepository>();
builder.Services.AddSingleton<ISchema, Schemes>();
builder.Services.AddSingleton<TaskGraphType>();
builder.Services.AddSingleton<CategoryGraphType>();
builder.Services.AddSingleton<Schemes>();
builder.Services.AddSingleton<Queries>();
builder.Services.AddSingleton<Mutations>();
builder.Services.AddSingleton<TaskInput>();
builder.Services.AddSingleton<TaskEdit>();
builder.Services.AddSingleton<CategoryInput>();
builder.Services.AddGraphQL(option => option.EnableMetrics = false)
                .AddSystemTextJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();

app.Run();
