using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
    public class SubscriptionType : ObjectGraphType<Subscription>
    {
        public SubscriptionType()
        {
            
            Field(o => o.Services, false, typeof(AService)).Resolve(ResolveServices);
        }

        private ICollection<AService> ResolveServices(ResolveFieldContext<Subscription> context)
        {
            var subscription = context.Source;
            return subscription.Services;
        }
    }
}
