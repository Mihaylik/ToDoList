using DataAccess.Data;
using DataAccess.Models;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XmlDataAccess.Repositories
{
    public class XmlTaskRepository : ITaskData
    {
        private const string XmlFilePath = "ToDoList.xml";
        private readonly XmlSerializer serializer;
        public XmlTaskRepository()
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
        public async Task DeleteTask(int idTask)
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
                TaskDbModel task = dataWrapper.tasks.Where(task => task.idTask == idTask).First();
                if (task == null)
                {
                    throw new Exception($"Task with id: {idTask} not found.");
                }
                dataWrapper.tasks.Remove(task);
                serializer.Serialize(xmlFile, dataWrapper);
            }
        }

        public Task<TaskDbModel> GetTask(int idTask)
        {
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                DataWrapper? dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if(dataWrapper == null)
                {
                    return null;
                }
                TaskDbModel task = dataWrapper.tasks.Where(task => task.idTask == idTask).First();
                if (task == null)
                {
                    throw new Exception($"Task with id: {idTask} not found.");
                }
                return Task.FromResult(task);
            }
        }

        public Task<IEnumerable<TaskDbModel>> GetTasks()
        {
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                DataWrapper? dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    return null;
                }
                List<TaskDbModel> tasks = dataWrapper.tasks;
                return Task.FromResult(tasks.AsEnumerable());
            }

        }

        public async Task InsertTask(TaskDbModel task)
        {
            DataWrapper? dataWrapper;
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if(dataWrapper == null)
                {
                    task.idTask = 1;
                    dataWrapper = new DataWrapper();
                    dataWrapper.tasks.Add(task);    
                }
                else
                {
                    task.idTask = dataWrapper.tasks.Count + 1;
                }

            }
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.Truncate))
            {
                dataWrapper.tasks.Add(task);
                serializer.Serialize(xmlFile, dataWrapper);
            }
        }

        public async Task UpdateTask(TaskDbModel task)
        {
            DataWrapper? dataWrapper;
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.OpenOrCreate))
            {
                dataWrapper = (DataWrapper?)serializer.Deserialize(xmlFile);
                if (dataWrapper == null)
                {
                    throw new Exception($"Task with id: {task.idTask} not found.");
                }
                TaskDbModel updatingTask = dataWrapper.tasks.Where(updatingTask => updatingTask.idTask == updatingTask.idTask).First();
                if (updatingTask == null)
                {
                    throw new Exception($"Task with id: {task.idTask} not found.");
                }
            }
            using (FileStream xmlFile = new FileStream(XmlFilePath, FileMode.Truncate))
            {
                int updatingTaskIndex = dataWrapper.tasks.FindIndex(updatingTask => updatingTask.idTask == task.idTask);
                dataWrapper.tasks[updatingTaskIndex] = task;
                serializer.Serialize(xmlFile, dataWrapper);
            }
        }
    }
}
