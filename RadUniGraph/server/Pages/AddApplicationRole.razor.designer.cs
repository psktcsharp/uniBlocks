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
    public partial class AddApplicationRoleComponent : ComponentBase
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
        public string newRole { get; set; }
        //IdentityRole _role;
        public IdentityRole role { get; set; }
        //{
        //    get
        //    {
        //        return _role;
        //    }
        //    set
        //    {
        //        if(!object.Equals(_role, value))
        //        {
        //            _role = value;
        //            InvokeAsync(() => { StateHasChanged(); });
        //        }
        //    }
        //}

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
            role = new IdentityRole();
        }

        protected async System.Threading.Tasks.Task Form0Submit()
        {
            try
            {
                role = new IdentityRole() { Name = newRole, NormalizedName = newRole.ToUpper() };
                var securityCreateRoleResult = await Security.CreateRole(role);
                UriHelper.NavigateTo("application-roles");
            }
            catch (Exception securityCreateRoleException)
            {
                    NotificationService.Notify(NotificationSeverity.Error, $"Cannot create role", $"{securityCreateRoleException.Message}");
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close();
            await JSRuntime.InvokeAsync<string>("window.history.back");
        }
    }
}
