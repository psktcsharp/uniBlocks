using GraphQL.Types;
using System.Linq;

namespace UniBlocks.Schemas.Portal
{
    internal class PortalQuery : ObjectGraphType
    {
        public PortalQuery(IPortal portal)
        {
            Field<ListGraphType<AServiceType>>("services", resolve: context => portal.AllServices.Take(100));
        }
    }
}