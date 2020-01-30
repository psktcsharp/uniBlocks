using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public AServiceSubscription AServiceSubscription { get; set; }
        public ATransaction Transaction { get; set; }
    }
}
