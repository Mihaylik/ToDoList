namespace ToDoListSANA.Models
{
    public class TaskViewModel
    {
        public int idTask { get; set; }
        public string? name { get; set; }
        public DateTime? timeStart { get; set; }
        public DateTime? deadline { get; set; }
        public bool passed { get; set; }
        public int idCategory { get; set; }
        public CategoryViewModel catagory { get; set; }
        public CategoryListViewModel categoryListModel { get; set; }
    }
}
