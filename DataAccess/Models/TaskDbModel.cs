namespace DataAccess.Models
{
    public class TaskDbModel
    {
        public int idTask { get; set; }
        public string? name { get; set; }
        public DateTime? timeStart { get; set; }
        public DateTime? deadline { get; set; }
        public bool passed { get; set; }
        public int idCategory { get; set; }

    }
}
