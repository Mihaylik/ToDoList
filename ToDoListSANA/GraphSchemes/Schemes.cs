using ToDoListSANA.GraphMutation;
using ToDoListSANA.GraphQueries;

namespace ToDoListSANA.GraphSchemes
{
    public class Schemes : Schema
    {
        public Schemes(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<Queries>();
            Mutation = serviceProvider.GetRequiredService<Mutations>();
        }
    }
}
