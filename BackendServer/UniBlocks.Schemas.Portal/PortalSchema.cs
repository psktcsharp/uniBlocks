using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
    public class PortalSchema : Schema
    {
        public PortalSchema(IPortal portal)
        {
            Query = new PortalQuery(portal);
            Mutation = new PortalMutation(portal);
            Subscription = new PortalSubscriptions(portal);
        }
    }
}
