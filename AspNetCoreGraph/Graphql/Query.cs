using AspNetCoreGraph;
using AspNetCoreGraph.Models;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;
using GraphQL.Types;

namespace AspNetCoreGraph.Graphql
{
    public class Query : ObjectGraphType
    {
        private Func<UniBlocksDBContext> uniDBContext;
        public Query( Func<UniBlocksDBContext> _uniDBContext)
        {
            uniDBContext = _uniDBContext;
        }
        [GraphQLMetadata("services")]
        public IEnumerable<AService> GetServices()
        {
            FieldAsync<NonNullGraphType<UserType>>(
              "user",
              resolve: async context =>
              {
                  using (var dc = dataContext())
                      return await dc
                          .Users
                          .FirstAsync(u => u.Id == context.Source.UserId);
              });

            return uniDBContext().Services;

            //using (var context = new UniBlocksDBContext(
            //  serviceProvider.GetRequiredService<
            //      DbContextOptions<RazorPagesMovieContext>>()))

            //    return uniDBContext.Services;
            //return uniBlocksDBContext.Services
            //    .Include(s => s.Subscriptions)
            //    .ToList();

        }
        [GraphQLMetadata("subscriptions")]
        public IEnumerable<Subscription> GetSubscriptions()
        {
            return new List<Subscription>();
            //using (var db = uniblocksdbcontext)
            //{
            //    return db.subscriptions
            //        .include(sub => sub.services)
            //        .tolist();
            //}
        }
        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return "World";
        }
    }
}