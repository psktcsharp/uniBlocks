﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using UniBlocksGraph.Models.UniSql;
using Microsoft.AspNetCore.Identity;
using UniBlocksGraph.Models;
namespace UniBlocksGraph.Layouts
{
    public partial class LoginLayoutComponent : LayoutComponentBase
    {
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


        protected RadzenBody body0;

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
        }

    }
}
