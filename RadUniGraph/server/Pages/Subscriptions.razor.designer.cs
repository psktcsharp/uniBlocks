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
    public partial class SubscriptionsComponent : ComponentBase
    {
        [Parameter]
        public int BlockId { get; set; }
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.Subscription> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.Subscription> _getSubscriptionsResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Subscription> getSubscriptionsResult
        {
            get
            {
                return _getSubscriptionsResult;
            }
            set
            {
                if(!object.Equals(_getSubscriptionsResult, value))
                {
                    _getSubscriptionsResult = value;
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
                await Load(BlockId);
            }

        }
        protected async System.Threading.Tasks.Task Load(int BlockId = 0)
        {
            var uniSqlGetSubscriptionsResult = await UniSql.GetSubscriptions(new Query() { Filter = "b => b.BlockId ==" + BlockId.ToString()});
            getSubscriptionsResult = uniSqlGetSubscriptionsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddSubscription>("Add Subscription", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.Subscription args)
        {
            var result = await DialogService.OpenAsync<EditSubscription>("Edit Subscription", new Dictionary<string, object>() { {"SubscriptionId", args.SubscriptionId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteSubscriptionResult = await UniSql.DeleteSubscription(data.SubscriptionId);
                if (uniSqlDeleteSubscriptionResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteSubscriptionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Subscription");
            }
        }
    }
}
