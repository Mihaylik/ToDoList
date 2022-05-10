using DataAccess.Data.Category;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoListSANA.Models;
using ToDoListSANA.Models.Category;

namespace ToDoListSANA.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskData taskData;
        private readonly ICategoryData categoryData;

        public TasksController(ITaskData data, ICategoryData categoryData)
        {
            this.taskData = data;
            this.categoryData = categoryData;
        }

        public async Task<IActionResult> Index(string categoryName)
        {
            var tasks = await taskData.GetTasks();
            var listTasks = new TaskListViewModel()
            {
                Tasks = new List<TaskViewModel>()
            };
            foreach (var task in tasks)
            {
                listTasks.Tasks.Add(await GetTaskViewModel(task.idTask));
            }
            var orderedListTasks = new TaskListViewModel()
            {
                Tasks = new List<TaskViewModel>()
            };
            orderedListTasks.Tasks
                .AddRange(listTasks.Tasks.Where(task => task.deadline != null && !task.passed/* && (idFilter == null) ? true :*/ )
                .OrderBy(task => task.deadline));
            orderedListTasks.Tasks.AddRange(listTasks.Tasks.Where(task => task.deadline == null/* && (idFilter == null) ? true : task.catagory.name == idFilter)*/));
            orderedListTasks.Tasks
                .AddRange(listTasks.Tasks.Where(task => task.deadline != null && task.passed/* && (idFilter == null) ? true : task.catagory.name == idFilter*/)
                .OrderBy(task => task.deadline));
            if (categoryName != null)
            {
                orderedListTasks.Tasks = orderedListTasks.Tasks.Where(task => task.catagory.name == categoryName).ToList();
            }
            return View(orderedListTasks);
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
            if (createdTask.catagory.name == null)
            {
                return RedirectToAction("Index");
            }
            var categorys = await categoryData.GetCategorys();
            var dbModel = new TaskDbModel()
            {
                name = createdTask.name,
                timeStart = createdTask.timeStart,
                deadline = createdTask.deadline,
                passed = createdTask.passed,
                idCategory = categorys.First(category => category.name == createdTask.catagory.name).idCategory
            };
            await taskData.InserTask(dbModel);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await GetTaskViewModel(id);
            var categories = await categoryData.GetCategorys();
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
            var categorys = await categoryData.GetCategorys();
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
        public async Task<IActionResult> Delete(int id)
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
    }
}