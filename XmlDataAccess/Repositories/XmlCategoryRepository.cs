using DataAccess.Data.Task;
using DataAccess.Models;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlDataAccess.Repositories
{
    public class XmlCategoryRepository : ICategoryData
    {
        private const string XmlFilePath = "ToDoList.xml";
        private readonly XmlSerializer serializer;
        public XmlCategoryRepository()
        {
            if (!File.Exists(XmlFilePath))
            {
                XDocument document = new XDocument();
                XElement dataWrapper = new XElement(nameof(DataWrapper));
                document.Add(dataWrapper);
                document.Save(XmlFilePath);
            }
            serializer = new XmlSerializer(typeof(DataWrapper));
        }
        public async Task DeleteCategory(int idCategory)
        {
            DataWrapper? dataWrapper;
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    return;
                }
            }
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.Truncate))
            {
                CategoryDbModel category = dataWrapper.categories.Where(category => category.idCategory == idCategory).First();
                if(category == null)
                {
                    throw new Exception($"Category with id: {idCategory} not found.");
                }
                dataWrapper.categories.Remove(category);
                serializer.Serialize(xmlFile, dataWrapper);
            }
        }

        public Task<CategoryDbModel> GetCategory(int idCategory)
        {
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                DataWrapper? dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    return null;
                }
                CategoryDbModel category = dataWrapper.categories.Where(category => category.idCategory == idCategory).First();
                if (category == null)
                {
                    throw new Exception($"Category with id: {idCategory} not found.");
                }
                return Task.FromResult(category);
            }
        }

        public Task<IEnumerable<CategoryDbModel>> GetCategories()
        {
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                DataWrapper? dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    return null;
                }
                List<CategoryDbModel> categories = dataWrapper.categories;
                return Task.FromResult(categories.AsEnumerable());
            }
        }

        public async Task InserCategory(CategoryDbModel category)
        {
            DataWrapper? dataWrapper;
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    category.idCategory = 1;
                    dataWrapper = new DataWrapper();
                    dataWrapper.categories.Add(category);
                }
                else
                {
                    category.idCategory = dataWrapper.categories.Count + 1;
                }

            }
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.Truncate))
            {
                dataWrapper.categories.Add(category);
                serializer.Serialize(xmlFile, dataWrapper);
            }
        }
    }
}
