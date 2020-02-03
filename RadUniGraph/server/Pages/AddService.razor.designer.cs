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
    public partial class AddServiceComponent : ComponentBase
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

        UniBlocksGraph.Models.UniSql.Service _service;
        protected UniBlocksGraph.Models.UniSql.Service service
        {
            get
            {
                return _service;
            }
            set
            {
                if(!object.Equals(_service, value))
                {
                    _service = value;
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
            service = new UniBlocksGraph.Models.UniSql.Service();
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.Service args)
        {
            try
            {
                var uniSqlCreateServiceResult = await UniSql.CreateService(service);
                DialogService.Close(service);
            }
            catch (Exception uniSqlCreateServiceException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Service!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
