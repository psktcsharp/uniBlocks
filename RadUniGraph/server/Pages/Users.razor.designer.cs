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
    public partial class UsersComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.User> grid0;

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
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddUser>("Add User", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.User args)
        {
            var result = await DialogService.OpenAsync<EditUser>("Edit User", new Dictionary<string, object>() { {"UserId", args.UserId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteUserResult = await UniSql.DeleteUser(data.UserId);
                if (uniSqlDeleteUserResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteUserException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete User");
            }
        }
    }
}
