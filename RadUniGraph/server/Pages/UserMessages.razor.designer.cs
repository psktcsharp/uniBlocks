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
    public partial class UserMessagesComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.UserMessage> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.UserMessage> _getUserMessagesResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.UserMessage> getUserMessagesResult
        {
            get
            {
                return _getUserMessagesResult;
            }
            set
            {
                if(!object.Equals(_getUserMessagesResult, value))
                {
                    _getUserMessagesResult = value;
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
            var uniSqlGetUserMessagesResult = await UniSql.GetUserMessages();
            getUserMessagesResult = uniSqlGetUserMessagesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddUserMessage>("Add User Message", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.UserMessage args)
        {
            var result = await DialogService.OpenAsync<EditUserMessage>("Edit User Message", new Dictionary<string, object>() { {"UserId", args.UserId}, {"MessageId", args.MessageId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteUserMessageResult = await UniSql.DeleteUserMessage(data.UserId, data.MessageId);
                if (uniSqlDeleteUserMessageResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteUserMessageException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete UserMessage");
            }
        }
    }
}
