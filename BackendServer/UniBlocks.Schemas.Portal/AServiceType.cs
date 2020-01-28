using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
   public class AServiceType : ObjectGraphType<AService>
    {
        public AServiceType()
        {
            Field(o => o.serviceName);
            Field(o => o.Subscriptions, false, typeof(Subscription)).Resolve(ResolveSubscriptions);
        }

        private ICollection<Subscription> ResolveSubscriptions(ResolveFieldContext<AService> context)
        {
            var aservice = context.Source;
            return aservice.Subscriptions;
        }
    }
}
