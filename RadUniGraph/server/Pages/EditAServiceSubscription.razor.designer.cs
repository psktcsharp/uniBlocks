﻿using System;
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
    public partial class EditAServiceSubscriptionComponent : ComponentBase
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
        public dynamic ServiceId { get; set; }

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

        UniBlocksGraph.Models.UniSql.AServiceSubscription _aservicesubscription;
        protected UniBlocksGraph.Models.UniSql.AServiceSubscription aservicesubscription
        {
            get
            {
                return _aservicesubscription;
            }
            set
            {
                if(!object.Equals(_aservicesubscription, value))
                {
                    _aservicesubscription = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        IEnumerable<UniBlocksGraph.Models.UniSql.Service> _getServicesResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Service> getServicesResult
        {
            get
            {
                return _getServicesResult;
            }
            set
            {
                if(!object.Equals(_getServicesResult, value))
                {
                    _getServicesResult = value;
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

            var uniSqlGetAServiceSubscriptionByServiceIdAndSubscriptionIdResult = await UniSql.GetAServiceSubscriptionByServiceIdAndSubscriptionId(int.Parse($"{ServiceId}"), int.Parse($"{SubscriptionId}"));
            aservicesubscription = uniSqlGetAServiceSubscriptionByServiceIdAndSubscriptionIdResult;

            var uniSqlGetServicesResult = await UniSql.GetServices();
            getServicesResult = uniSqlGetServicesResult;

            var uniSqlGetSubscriptionsResult = await UniSql.GetSubscriptions();
            getSubscriptionsResult = uniSqlGetSubscriptionsResult;
        }

        protected async System.Threading.Tasks.Task CloseButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.AServiceSubscription args)
        {
            try
            {
                var uniSqlUpdateAServiceSubscriptionResult = await UniSql.UpdateAServiceSubscription(int.Parse($"{ServiceId}"), int.Parse($"{SubscriptionId}"), aservicesubscription);
                DialogService.Close(aservicesubscription);
            }
            catch (Exception uniSqlUpdateAServiceSubscriptionException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update AServiceSubscription");
            }
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
