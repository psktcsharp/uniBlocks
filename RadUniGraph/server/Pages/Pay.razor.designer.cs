using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using UniBlocksGraph.Models.UniSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UniBlocksGraph.Models;

namespace UniBlocksGraph.Pages
{

    public partial class PayComponent : ComponentBase
    {
        //[Parameter]
        //public int ServiceId { get; set; }
        //[Parameter]
        //public int SubId { get; set; }
        [Inject]
        protected UniSqlService UniSql { get; set; }

        public Service service { get; set; }
        public Subscription sub { get; set; }
        public decimal amount { get; set; }
        public string result { get; set; }

    }
}
