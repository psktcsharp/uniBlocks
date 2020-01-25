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
        [GraphQLMetadata("services")]
        public IEnumerable<Service> GetServices()
        {
            using(var db = new UniBlocksDBContext())
            {
                return db.Services
                    .Include(s => s.Subscriptions)
                    .ToList();
            }
        }
        [GraphQLMetadata("subscriptions")]
        public IEnumerable<Subscription> GetSubscriptions()
        {
            using (var db = new UniBlocksDBContext())
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