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
    public partial class AddUserMessageComponent : ComponentBase
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

        UniBlocksGraph.Models.UniSql.UserMessage _usermessage;
        protected UniBlocksGraph.Models.UniSql.UserMessage usermessage
        {
            get
            {
                return _usermessage;
            }
            set
            {
                if(!object.Equals(_usermessage, value))
                {
                    _usermessage = value;
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

            var uniSqlGetMessagesResult = await UniSql.GetMessages();
            getMessagesResult = uniSqlGetMessagesResult;

            usermessage = new UniBlocksGraph.Models.UniSql.UserMessage();
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.UserMessage args)
        {
            try
            {
                var uniSqlCreateUserMessageResult = await UniSql.CreateUserMessage(usermessage);
                DialogService.Close(usermessage);
            }
            catch (Exception uniSqlCreateUserMessageException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to create new UserMessage!");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
