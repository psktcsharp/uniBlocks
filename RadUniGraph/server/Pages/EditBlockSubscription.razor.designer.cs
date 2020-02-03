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
    public partial class EditBlockSubscriptionComponent : ComponentBase
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

        [Parameter]
        public dynamic BlockId { get; set; }

        [Parameter]
        public dynamic SubscriptionId { get; set; }

        bool _canEdit;
        protected bool canEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                if(!object.Equals(_canEdit, value))
                {
                    _canEdit = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        UniBlocksGraph.Models.UniSql.BlockSubscription _blocksubscription;
        protected UniBlocksGraph.Models.UniSql.BlockSubscription blocksubscription
        {
            get
            {
                return _blocksubscription;
            }
            set
            {
                if(!object.Equals(_blocksubscription, value))
                {
                    _blocksubscription = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

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
                await Load();
            }

        }
        protected async System.Threading.Tasks.Task Load()
        {
            canEdit = true;

            var uniSqlGetBlockSubscriptionByBlockIdAndSubscriptionIdResult = await UniSql.GetBlockSubscriptionByBlockIdAndSubscriptionId(int.Parse($"{BlockId}"), int.Parse($"{SubscriptionId}"));
            blocksubscription = uniSqlGetBlockSubscriptionByBlockIdAndSubscriptionIdResult;

            var uniSqlGetBlocksResult = await UniSql.GetBlocks();
            getBlocksResult = uniSqlGetBlocksResult;

            var uniSqlGetSubscriptionsResult = await UniSql.GetSubscriptions();
            getSubscriptionsResult = uniSqlGetSubscriptionsResult;
        }

        protected async System.Threading.Tasks.Task CloseButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.BlockSubscription args)
        {
            try
            {
                var uniSqlUpdateBlockSubscriptionResult = await UniSql.UpdateBlockSubscription(int.Parse($"{BlockId}"), int.Parse($"{SubscriptionId}"), blocksubscription);
                DialogService.Close(blocksubscription);
            }
            catch (Exception uniSqlUpdateBlockSubscriptionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update BlockSubscription");
            }
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
