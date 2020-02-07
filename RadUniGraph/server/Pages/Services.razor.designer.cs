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
    public partial class ServicesComponent : ComponentBase
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
        public int BlockIdForService { get; set; }
        [Parameter]
        public int SubIdForService { get; set; }

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.Service> grid0;

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load(BlockIdForService,SubIdForService);
            }

        }
        protected async System.Threading.Tasks.Task Load(int BlockIdForService = 0,int SubIdForService =0)
        {
            var uniSqlGetServicesResult = await UniSql.GetServices(new Query() { Filter = BlockIdForService.ToString() });
            getServicesResult = uniSqlGetServicesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddService>("Add Service", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.Service args)
        {
            var result = await DialogService.OpenAsync<EditService>("Edit Service", new Dictionary<string, object>() { {"AServiceId", args.AServiceId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteServiceResult = await UniSql.DeleteService(data.AServiceId);
                if (uniSqlDeleteServiceResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteServiceException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Service");
            }
        }
        protected async System.Threading.Tasks.Task GridPayButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                
                //var uniSqlDeleteServiceResult = await UniSql.DeleteService(data.AServiceId);
                //if (uniSqlDeleteServiceResult != null)
                //{
                //    grid0.Reload();
                //}

    }
            catch (Exception uniSqlDeleteServiceException)
            {
                NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Service");
            }
        }
    }
}
