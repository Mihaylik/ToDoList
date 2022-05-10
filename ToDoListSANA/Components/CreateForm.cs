using DataAccess.Data.Category;
using Microsoft.AspNetCore.Mvc;
using ToDoListSANA.Models;
using ToDoListSANA.Models.Category;

namespace ToDoListSANA.Components
{
    public class CreateForm : ViewComponent
    {
        private readonly ICategoryData categoryData;

        public CreateForm(ICategoryData categoryData)
        {
            this.categoryData = categoryData;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TaskViewModel();
            model.categoryListModel = new CategoryListViewModel();

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
            return View("CreateForm",model);
        }
    }
}
