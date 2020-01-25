using GraphQL.Types;
using GraphQL;

namespace AspNetCoreGraph.Graphql
{
    public class MySchema
    {
        private ISchema _schema { get; set; }
        public ISchema GraphQLSchema
        {
            get
            {
                return this._schema;
            }
        }

        public MySchema()
        {
            this._schema = Schema.For(@"
          type Subscription {
            id: ID
            services: [Service]
          }

          type Service {
            id: ID,
            subscriptions: [Subscription]
          }

          type Mutation {
            addService(name: String): Service
          }

          type Query {
              services: [Service]
              subscriptions: [Subscription]
          }
      ", _ =>
            {
                _.Types.Include<Query>();
                _.Types.Include<Mutation>();
            });
        }

    }
}