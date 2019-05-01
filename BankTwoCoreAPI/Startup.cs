using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTwoAPI_Data.ConcreteClasses;
using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using BankTwoCoreAPI.UtilityLogic.Email;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BankTwoCoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //var ConnectionString = @"Data Source=GABRIEL\SQLEXPRESS;Initial Catalog=BankTwoCoreAPI_DB;Integrated Security=True";
            services.AddDbContext<BankTwoDatabase>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            services.AddOData();

            services.AddAuthentication(p =>
           {
               p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           }).AddJwtBearer(p =>
           {
               p.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var customerService = context.HttpContext.RequestServices.GetRequiredService<ICustomerRepo<Customer>>();
                       return Task.CompletedTask;
                   }
               };
               //FIXME: Finish Authentication [Startup.cs] 
               p.RequireHttpsMetadata = false;
               p.SaveToken = true;
        


           });
            //Dependency Constructor Injection
            services.AddSingleton<IEmailSender,EmailSender>();

            services.AddTransient<IBankRepo<Banks>, BankRepo>();
            services.AddTransient<ICustomerAccountRepo<CustomerAccount>, CustomerAccountRepo>();
            services.AddTransient<ICustomerBeneficiaryRepo<CustomerBeneficiary>, CustomerBeneficiaryRepo>();
            services.AddTransient<ICustomerRepo<Customer>, CustomerRepo>();
            services.AddTransient<ICardRequestRepo<CardRequest>,CardRequestRepo>();
            services.AddTransient<IAirtimeTopUpRepo<AirTimeTopUp>, AirtimeTopUpRepo>();
            services.AddTransient<IInterBankTransferRepo<InterBankTransfer>,InterBankTransferRepo>();
            services.AddTransient<IIntraBankTransferRepo<IntraBankTransfer>,IntraBankTransferRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

         
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();
                routeBuilder.Select().OrderBy().Filter().Count().Expand();
            });
            
        }
    }
}
