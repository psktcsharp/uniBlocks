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
    public partial class BlockUsersComponent : ComponentBase
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

        protected RadzenGrid<UniBlocksGraph.Models.UniSql.BlockUser> grid0;

        IEnumerable<UniBlocksGraph.Models.UniSql.BlockUser> _getBlockUsersResult;
        protected IEnumerable<UniBlocksGraph.Models.UniSql.BlockUser> getBlockUsersResult
        {
            get
            {
                return _getBlockUsersResult;
            }
            set
            {
                if(!object.Equals(_getBlockUsersResult, value))
                {
                    _getBlockUsersResult = value;
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
            var uniSqlGetBlockUsersResult = await UniSql.GetBlockUsers();
            getBlockUsersResult = uniSqlGetBlockUsersResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var result = await DialogService.OpenAsync<AddBlockUser>("Add Block User", null);
              grid0.Reload();

              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(UniBlocksGraph.Models.UniSql.BlockUser args)
        {
            var result = await DialogService.OpenAsync<EditBlockUser>("Edit Block User", new Dictionary<string, object>() { {"BlockId", args.BlockId}, {"UserId", args.UserId} });
              await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                var uniSqlDeleteBlockUserResult = await UniSql.DeleteBlockUser(data.BlockId, data.UserId);
                if (uniSqlDeleteBlockUserResult != null) {
                    grid0.Reload();
}
            }
            catch (Exception uniSqlDeleteBlockUserException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Error", $"Unable to delete BlockUser");
            }
        }
    }
}
