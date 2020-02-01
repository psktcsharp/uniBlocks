using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotGraphApi.UniBlocks.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniBlocks.Schemas.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotGraphApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<UniBlocksDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDataRepository, DataRepository>();
            //  services.addser<IDataRepository, DataRepository>();
            services.AddGraphQL(sp => SchemaBuilder.New()
            .AddServices(sp)
                .AddQueryType(d => d.Name("Query"))
                 .AddMutationType(d => d.Name("Mutation"))
                .AddType<AllQueries>()
                .AddType<AllMutations>()
                .Create());       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Create the database if it doesn't exist
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGraphQL();
            app.UsePlayground();
            
        }
        
    }
}
