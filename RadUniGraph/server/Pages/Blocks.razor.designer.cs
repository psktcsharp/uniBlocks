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
    public partial class BlocksComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }


        [Inject]
        protected UniSqlService UniSql { get; set; }

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.Block> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.Block> _getBlocksResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Block> getBlocksResult
        {
            get
            {
                return _getBlocksResult;
            }
            set
            {
                if(!object.Equals(_getBlocksResult, value))
                {
                    _getBlocksResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            var uniSqlGetBlocksResult = await UniSql.GetBlocks();
            getBlocksResult = uniSqlGetBlocksResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddBlock>("Add Block", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.Block args)
        {
            var result = await DialogService.OpenAsync<EditBlock>("Edit Block", new Dictionary<string, object>() { {"BlockId", args.BlockId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteBlockResult = await UniSql.DeleteBlock(data.BlockId);
                if (uniSqlDeleteBlockResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteBlockException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Block");
            }
        }
    }
}
