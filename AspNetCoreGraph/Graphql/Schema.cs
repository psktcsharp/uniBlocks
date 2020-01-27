using GraphQL.Types;
using GraphQL;


namespace AspNetCoreGraph.Graphql
{
    public class MySchema : GraphQL.Types.Schema
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
            id: ID,
            services: [AService]
          }

          type AService {
            id: ID,
            Name: String
            subscriptions: [Subscription]
          }

          type Mutation {
            addService(name: String): AService
          }

          type Query {
              services: [AService]
              subscriptions: [Subscription]
                hello:String
          }
      ", _ =>
            {
                _.Types.Include<Query>();
                _.Types.Include<Mutation>();
            });
        }

    }
}