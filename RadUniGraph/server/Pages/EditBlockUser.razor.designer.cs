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
    public partial class EditBlockUserComponent : ComponentBase
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
        public dynamic UserId { get; set; }

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

        UniBlocksGraph.Models.UniSql.BlockUser _blockuser;
        protected UniBlocksGraph.Models.UniSql.BlockUser blockuser
        {
            get
            {
                return _blockuser;
            }
            set
            {
                if(!object.Equals(_blockuser, value))
                {
                    _blockuser = value;
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
            canEdit = true;

            var uniSqlGetBlockUserByBlockIdAndUserIdResult = await UniSql.GetBlockUserByBlockIdAndUserId(int.Parse($"{BlockId}"), int.Parse($"{UserId}"));
            blockuser = uniSqlGetBlockUserByBlockIdAndUserIdResult;

            var uniSqlGetBlocksResult = await UniSql.GetBlocks();
            getBlocksResult = uniSqlGetBlocksResult;

            var uniSqlGetUsersResult = await UniSql.GetUsers();
            getUsersResult = uniSqlGetUsersResult;
        }

        protected async System.Threading.Tasks.Task CloseButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async System.Threading.Tasks.Task Form0Submit(UniBlocksGraph.Models.UniSql.BlockUser args)
        {
            try
            {
                var uniSqlUpdateBlockUserResult = await UniSql.UpdateBlockUser(int.Parse($"{BlockId}"), int.Parse($"{UserId}"), blockuser);
                DialogService.Close(blockuser);
            }
            catch (Exception uniSqlUpdateBlockUserException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to update BlockUser");
            }
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
