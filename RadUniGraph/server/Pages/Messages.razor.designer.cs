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
    public partial class MessagesComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.Message> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.Message> _getMessagesResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.Message> getMessagesResult
        {
            get
            {
                return _getMessagesResult;
            }
            set
            {
                if(!object.Equals(_getMessagesResult, value))
                {
                    _getMessagesResult = value;
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
            var uniSqlGetMessagesResult = await UniSql.GetMessages();
            getMessagesResult = uniSqlGetMessagesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddMessage>("Add Message", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.Message args)
        {
            var result = await DialogService.OpenAsync<EditMessage>("Edit Message", new Dictionary<string, object>() { {"MessageId", args.MessageId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteMessageResult = await UniSql.DeleteMessage(data.MessageId);
                if (uniSqlDeleteMessageResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteMessageException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete Message");
            }
        }
    }
}
