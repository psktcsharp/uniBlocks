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
    public partial class BalancesComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.Balance> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.Balance> _getBalancesResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Balance> getBalancesResult
        {
            get
            {
                return _getBalancesResult;
            }
            set
            {
                if(!object.Equals(_getBalancesResult, value))
                {
                    _getBalancesResult = value;
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
            var uniSqlGetBalancesResult = await UniSql.GetBalances();
            getBalancesResult = uniSqlGetBalancesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddBalance>("Add Balance", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.Balance args)
        {
            var result = await DialogService.OpenAsync<EditBalance>("Edit Balance", new Dictionary<string, object>() { {"BalanceId", args.BalanceId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteBalanceResult = await UniSql.DeleteBalance(data.BalanceId);
                if (uniSqlDeleteBalanceResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteBalanceException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Balance");
            }
        }
    }
}
