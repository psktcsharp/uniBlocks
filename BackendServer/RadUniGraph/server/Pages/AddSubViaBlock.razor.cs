using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniBlocksGraph.Models.UniSql;
namespace UniBlocksGraph.Pages
{
    public partial class AddSubViaBlock : ComponentBase
    {
        [Inject]
        protected UniSqlService UniSql { get; set; }

        public Service service { get; set; }
        public Block block { get; set; }
  
        public string newSubName { get; set; }
        public bool isActive { get; set; }
        public int UserId { get; set; }
        [CascadingParameter] ModalParameters Parameters { get; set; }
        IEnumerable<UniBlocksGraph.Models.UniSql.User> _getUsersResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.User> getUsersResult
        {
            get
            {
                return _getUsersResult.Distinct();
            }
            set
            {
                if (!object.Equals(_getUsersResult, value))
                {
                    _getUsersResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }
    }

}
