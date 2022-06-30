using DataAccess.Data.Task;
using DataAccess.Models;
using SqlDataAccess.Data;
using ToDoListSANA.Enums;
using XmlDataAccess.Repositories;

namespace ToDoListSANA.Switcher
{
    public static class RepositorySwitcher
    {
        public static T GetPropered<T>(this IEnumerable<T> repositories, DataProvider dataProvider) where T : IData
        {
            switch (dataProvider)
            {
                case DataProvider.Xml:
                    if (repositories is IEnumerable<ITaskData>)
                    {
                        return repositories.ElementAt(1);
                    }
                    else if (repositories is IEnumerable<ICategoryData>)
                    {
                        return repositories.ElementAt(1);
                    }
                    else
                    {
                        throw new Exception("Not Exist");
                    }

                case DataProvider.Database:
                default:
                    if (repositories is IEnumerable<ITaskData>)
                    {
                        return repositories.ElementAt(0);
                    }
                    else if (repositories is IEnumerable<ICategoryData>)
                    {
                        return repositories.ElementAt(0);
                    }
                    else
                    {
                        throw new Exception("Not Exist");
                    }
            }
        }
    }
}
