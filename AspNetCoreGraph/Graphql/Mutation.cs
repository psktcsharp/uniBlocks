using AspNetCoreGraph.Models;
using GraphQL;

namespace AspNetCoreGraph.Graphql
{
    [GraphQLMetadata("Mutation")]
    public class Mutation
    {
        private UniBlocksDBContext uniBlocksDBContext;
        public Mutation(UniBlocksDBContext uniCont)
        {
            uniBlocksDBContext = uniCont;
        }
        [GraphQLMetadata("addService")]
        public AService Add(string name)
        {
            using (var db = uniBlocksDBContext)
            {
                var service = new AService() { Name = name };
                db.Services.Add(service);
                db.SaveChanges();
                return service;
            }
        }
    }
}