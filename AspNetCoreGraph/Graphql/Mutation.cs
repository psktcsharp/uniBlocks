using AspNetCoreGraph.Models;
using GraphQL;

namespace AspNetCoreGraph.Graphql
{
    [GraphQLMetadata("Mutation")]
    public class Mutation
    {
        [GraphQLMetadata("Mutation")]
        public Service Add(string name)
        {
            using(var db = new UniBlocksDBContext())
            {
                var service = new Service() { Name = name };
                db.Services.Add(service);
                db.SaveChanges();
                return service;
            }
        }
    }
}