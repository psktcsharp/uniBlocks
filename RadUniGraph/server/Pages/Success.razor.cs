using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniBlocksGraph.Pages
{
    public partial class Success : ComponentBase
    {
        [CascadingParameter] ModalParameters Parameters { get; set; }
        [Parameter]
        public string msg { get; set; }
    }
}
