using DataAccess.Data.Task;
using Microsoft.AspNetCore.Mvc;
using ToDoListSANA.Enums;
using ToDoListSANA.Models;
using ToDoListSANA.Switcher;

namespace ToDoListSANA.Components
{
    public class CreateForm : ViewComponent
    {
        private readonly ICategoryData categoryData;

        public CreateForm(IEnumerable<ICategoryData> categoryData, IHttpContextAccessor contextAccessor)
        {
            DataProvider dataProvider;
            Enum.TryParse(contextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            this.categoryData = categoryData.GetPropered(dataProvider);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TaskViewModel();
            model.categoryListModel = new CategoryListViewModel();

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
            return View("CreateForm",model);
        }
    }
}
