﻿using AspNetCoreGraph.Models;
using GraphQL;
using System.Collections;
using System.Collections.Generic;

namespace Api.Graphql
{
    public class Query
    {
        [GraphQLMetadata("services")]
        public IEnumerable<Service> GetServices()
        {
            using(var db = new UniBlocksDBContext())
            {
                return db.Services
                    .Include(s => s.Subscription)
                    .ToList();
            }
        }
        public IEnumerable<Service> GetSubscriptions()
        {
            using (var db = new UniBlocksDBContext())
            {
                return db.Subscriptions
                    .Include(sub => sub.Services)
                    .ToList();
            }
        }
    }
}