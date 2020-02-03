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
    public partial class AServiceSubscriptionsComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.AServiceSubscription> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.AServiceSubscription> _getAServiceSubscriptionsResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.AServiceSubscription> getAServiceSubscriptionsResult
        {
            get
            {
                return _getAServiceSubscriptionsResult;
            }
            set
            {
                if(!object.Equals(_getAServiceSubscriptionsResult, value))
                {
                    _getAServiceSubscriptionsResult = value;
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
            var uniSqlGetAServiceSubscriptionsResult = await UniSql.GetAServiceSubscriptions();
            getAServiceSubscriptionsResult = uniSqlGetAServiceSubscriptionsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddAServiceSubscription>("Add A Service Subscription", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.AServiceSubscription args)
        {
            var result = await DialogService.OpenAsync<EditAServiceSubscription>("Edit A Service Subscription", new Dictionary<string, object>() { {"ServiceId", args.ServiceId}, {"SubscriptionId", args.SubscriptionId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteAServiceSubscriptionResult = await UniSql.DeleteAServiceSubscription(data.ServiceId, data.SubscriptionId);
                if (uniSqlDeleteAServiceSubscriptionResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteAServiceSubscriptionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete AServiceSubscription");
            }
        }
    }
}
