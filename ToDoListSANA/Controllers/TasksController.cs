using DataAccess.Data.Task;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using ToDoListSANA.Enums;
using ToDoListSANA.Models;
using ToDoListSANA.Switcher;

namespace ToDoListSANA.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskData taskData;
        private readonly ICategoryData categoryData;

        public TasksController(IEnumerable<ITaskData> taskData, IEnumerable<ICategoryData> categoryData, IHttpContextAccessor contextAccessor)
        {
            DataProvider dataProvider;
            Enum.TryParse(contextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            this.taskData = taskData.GetPropered(dataProvider);
            this.categoryData = categoryData.GetPropered(dataProvider);
        }

        public async Task<IActionResult> Index(string categoryName)
        {
            var tasks = await taskData.GetTasks();
            var categories = await categoryData.GetCategories();
            var listTasks = new TaskListViewModel()
            {
                tasks = new List<TaskViewModel>()
            };
            foreach (var task in tasks)
            {
                listTasks.tasks.Add(await GetTaskViewModel(task.idTask));
            }
            var orderedListTasks = new TaskListViewModel()
            {
                tasks = new List<TaskViewModel>()
            };
            orderedListTasks.tasks
                .AddRange(listTasks.tasks.Where(task => task.deadline != null && !task.passed)
                .OrderBy(task => task.deadline));
            orderedListTasks.tasks
                .AddRange(listTasks.tasks.Where(task => task.deadline == null));
            orderedListTasks.tasks
                .AddRange(listTasks.tasks.Where(task => task.deadline != null && task.passed)
                .OrderBy(task => task.deadline));
            if (categoryName != null)
            {
                orderedListTasks.tasks = orderedListTasks.tasks
                    .Where(task => task.catagory.name == categoryName).ToList();
            }
            return View(orderedListTasks);
        }
        [HttpPost]
        public IActionResult ChangeDataProvider(DataProvider dataProvider)
        {
            Response.Cookies.Append("DataProvider", dataProvider.ToString());
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Datails(int id)
        {
            var model = await GetTaskViewModel(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(TaskViewModel createdTask)
        {
            if (createdTask.name == null)
            {
                return RedirectToAction("Index");
            }
            var categorys = await categoryData.GetCategories();
            var dbModel = new TaskDbModel()
            {
                name = createdTask.name,
                timeStart = createdTask.timeStart,
                deadline = createdTask.deadline,
                passed = createdTask.passed,
                idCategory = categorys.First(category => category.name == createdTask.catagory.name).idCategory
            };
            await taskData.InsertTask(dbModel);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await GetTaskViewModel(id);
            var categories = await categoryData.GetCategories();
            model.categoryListModel = new CategoryListViewModel();
            model.categoryListModel.categories = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                model.categoryListModel.categories.Add(new CategoryViewModel()
                {
                    idCategory = category.idCategory,
                    name = category.name
                });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskViewModel editedTask)
        {
            var categorys = await categoryData.GetCategories();
            var dbModel = new TaskDbModel()
            {
                idTask = editedTask.idTask,
                name = editedTask.name,
                timeStart = editedTask.timeStart,
                deadline = editedTask.deadline,
                passed = editedTask.passed,
                idCategory = categorys.First(category => category.name == editedTask.catagory.name).idCategory
            };
            await taskData.UpdateTask(dbModel);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await taskData.DeleteTask(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View("/Views/Category/CreateCategory.cshtml", new CategoryViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryViewModel category)
        {
            if (category.name == null)
            {
                return RedirectToAction("Index");
            }
            var dbModel = new CategoryDbModel()
            {
                name = category.name
            };
            await categoryData.InserCategory(dbModel);
            return RedirectToAction("Index");
        }
        public async Task<TaskViewModel> GetTaskViewModel(int id)
        {
            var task = await taskData.GetTask(id);
            var category = await categoryData.GetCategory((int)task.idCategory);
            return new TaskViewModel
            {
                idTask = id,
                name = task.name,
                timeStart = task.timeStart,
                deadline = task.deadline,
                passed = task.passed,
                catagory = new CategoryViewModel
                {
                    idCategory = category.idCategory,
                    name = category.name
                }
            };
        }
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await categoryData.GetCategories();
            var listCategories = new CategoryListViewModel()
            {
                categories = new List<CategoryViewModel>()
            };
            foreach(var category in categories)
            {
                listCategories.categories.Add(await GetCategoryViewModel(category.idCategory));
            }
            return View("Views/Category/Categories.cshtml", listCategories);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var tasks = await taskData.GetTasks();
            var tasksWithThisCategory = tasks.Where(task => task.idCategory == id);
            foreach (var task in tasksWithThisCategory)
            {
                await taskData.DeleteTask(task.idTask);
            }
            await categoryData.DeleteCategory(id);
            return RedirectToAction("Views/Category/Categories.cshtml");
        }
        public async Task<CategoryViewModel> GetCategoryViewModel(int id)
        {
            var category = await categoryData.GetCategory(id);
            return new CategoryViewModel
            {
                idCategory = category.idCategory,
                name = category.name
            };
        }

    }
}