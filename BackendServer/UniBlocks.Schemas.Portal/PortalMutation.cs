using GraphQL.Types;

namespace UniBlocks.Schemas.Portal
{
    internal class PortalMutation : ObjectGraphType<object>
    {
        public PortalMutation(IPortal portal)
        {
            Field<AServiceType>("addService",
                arguments: new QueryArguments(
                    new QueryArgument<AServiceInputType>
                {
                    Name = "AService"
                }),
                resolve: context =>
                {
                    var inputService = context.GetArgument<AService>("AService");
                    var aservice = portal.AddAService(inputService);
                    return aservice;
                });
                 
        }
    }

    public class AServiceInputType : InputObjectGraphType
    {
        public AServiceInputType()
        {
            Field<StringGraphType>("serviceName");
        }
    }
}