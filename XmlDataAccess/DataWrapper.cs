using DataAccess.Models;

namespace XmlDataAccess
{
    public class DataWrapper
    {
        public List<TaskDbModel> tasks { get; set; }
        public List<CategoryDbModel> categories { get; set; }
    }

}
