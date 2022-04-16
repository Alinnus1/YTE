using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTE.Code;
using YTE.Code.ExtensionMethods;
using YTE.DataAccess;
using YTE.Entities.Context;
using YTE.BusinessLogic.Base;
using AutoMapper;
using YTE.Common;
using Hangfire;
using Hangfire.SqlServer;
using YTE.WebApp.Jobs;


namespace YTE
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
            });
            services.AddAutoMapper(options =>
            {
                options.AddMaps(typeof(Startup), typeof(BaseService));
            });

            services.AddDbContext<YTEContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddScoped<UnitOfWork>();
            services.Configure<MailSenderConfig>(Configuration.GetSection(nameof(MailSenderConfig)));


            services.AddYTECurrentUser();

            services.AddPresentation();

            services.AddYTEBusinessLogic();
            services.AddRazorPages();

            services.AddAuthentication("YTECookies")
                   .AddCookie("YTECookies", options =>
                   {
                       options.AccessDeniedPath = new PathString("/Error/Error_Unauthorized");
                       options.LoginPath = new PathString("/UserAccount/Login");
                   });

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(10),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            services.AddHangfireServer();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, IServiceProvider serviceProvider)

        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();

            BackgroundJobs.CreateJobs(serviceProvider);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();

                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{type?}");
            });
        }
    }
}
