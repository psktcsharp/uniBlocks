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
    public partial class AddInvoiceComponent : ComponentBase
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

        IEnumerable<UniBlocksGraph.Models.UniSql.Transaction> _getTransactionsResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Transaction> getTransactionsResult
        {
            get
            {
                return _getTransactionsResult;
            }
            set
            {
                if(!object.Equals(_getTransactionsResult, value))
                {
                    _getTransactionsResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        UniBlocksGraph.Models.UniSql.Invoice _invoice;
        protected UniBlocksGraph.Models.UniSql.Invoice invoice
        {
            get
            {
                return _invoice;
            }
            set
            {
                if(!object.Equals(_invoice, value))
                {
                    _invoice = value;
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

            var uniSqlGetTransactionsResult = await UniSql.GetTransactions();
            getTransactionsResult = uniSqlGetTransactionsResult;

            invoice = new UniBlocksGraph.Models.UniSql.Invoice();
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.Invoice args)
        {
            try
            {
                var uniSqlCreateInvoiceResult = await UniSql.CreateInvoice(invoice);
                DialogService.Close(invoice);
            }
            catch (Exception uniSqlCreateInvoiceException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Invoice!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
