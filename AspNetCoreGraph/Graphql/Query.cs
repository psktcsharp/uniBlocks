using AspNetCoreGraph;
using AspNetCoreGraph.Models;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreGraph.Graphql
{
    public class Query
    {
        private UniBlocksDBContext uniBlocksDBContext;
        public Query(UniBlocksDBContext uniCont)
        {
            uniBlocksDBContext = uniCont;
        }
        [GraphQLMetadata("services")]
        public IEnumerable<AService> GetServices()
        {
          
                return uniBlocksDBContext.Services
                    .Include(s => s.Subscriptions)
                    .ToList();
            
        }
        [GraphQLMetadata("subscriptions")]
        public IEnumerable<Subscription> GetSubscriptions()
        {
            using (var db = uniBlocksDBContext)
            {
                return db.Subscriptions
                    .Include(sub => sub.Services)
                    .ToList();
            }
        }
        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return "World";
        }
    }
}