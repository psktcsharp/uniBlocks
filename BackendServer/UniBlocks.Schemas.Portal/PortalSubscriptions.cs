using GraphQL.Types;

namespace UniBlocks.Schemas.Portal
{
    internal class PortalSubscriptions : ObjectGraphType<object>
    {
        private IPortal portal;

        public PortalSubscriptions(IPortal portal)
        {
            this.portal = portal;
        }
    }
}