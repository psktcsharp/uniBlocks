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
    public partial class AddMessageComponent : ComponentBase
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

        IEnumerable<UniBlocksGraph.Models.UniSql.User> _getUsersResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.User> getUsersResult
        {
            get
            {
                return _getUsersResult;
            }
            set
            {
                if(!object.Equals(_getUsersResult, value))
                {
                    _getUsersResult = value;
                    InvokeAsync(() => { StateHasChanged(); });
                }
            }
        }

        UniBlocksGraph.Models.UniSql.Message _message;
        protected UniBlocksGraph.Models.UniSql.Message message
        {
            get
            {
                return _message;
            }
            set
            {
                if(!object.Equals(_message, value))
                {
                    _message = value;
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
            var uniSqlGetUsersResult = await UniSql.GetUsers();
            getUsersResult = uniSqlGetUsersResult;

            message = new UniBlocksGraph.Models.UniSql.Message();
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.Message args)
        {
            try
            {
                var uniSqlCreateMessageResult = await UniSql.CreateMessage(message);
                DialogService.Close(message);
            }
            catch (Exception uniSqlCreateMessageException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new Message!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
