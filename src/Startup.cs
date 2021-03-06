using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomOnboardingProvider
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // NOTE on services.Add????? dependency injection
            // Transient objects are always different; a new instance is provided to every controller and every service.
            // Scoped objects are the same within a request, but different across different requests.
            // Singleton objects are the same for every object and every request.

            // This has to be singleton as AccountController creates user, while profile service accesses this data later
            services.AddSingleton<IUserStore<ApplicationUser>, TransientUserStore>();
            // services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, TransientUserClaimsPrincipalFactory>();
            services.AddScoped<IProfileService, UserProfileService>();
            // services.AddScoped<IResourceOwnerPasswordValidator, TransientResourceOwnerPasswordValidator>();

            // Expect reverse proxy in front of this app
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // services.AddDbContext<ApplicationDbContext>(builder =>
            //     builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

            // services.AddIdentity<IdentityUser, IdentityRole>();
            // .AddEntityFrameworkStores<ApplicationDbContext>();

            var signingFileName = Environment.GetEnvironmentVariable("OIDC_KEYPAIR_PFX_FILE");

            if (String.IsNullOrEmpty(signingFileName) || !File.Exists(signingFileName))
            {
                throw new FileNotFoundException("Signing certificate is missing! Please provide valid value to OIDC_KEYPAIR_PFX_FILE.");
            }

            var siginingKeyPass = Environment.GetEnvironmentVariable("OIDC_KEYPAIR_PASS");

            if (String.IsNullOrEmpty(signingFileName))
            {
                throw new ArgumentNullException("Signing certificate password is missing! Please provide valid value to OIDC_KEYPAIR_PASS.");
            }

            var cert = new X509Certificate2(signingFileName, siginingKeyPass);

            var ids = services.AddIdentityServer()
                .AddInMemoryClients(OidcClients.Get())
                .AddInMemoryIdentityResources(OidcResources.GetIdentityResources())
                .AddProfileService<UserProfileService>()
                // .AddAspNetIdentity<ApplicationUser>()
                // .AddTestUsers(Users.Get())
                .AddSigningCredential(cert)
                // .AddDeveloperSigningCredential();
                ;


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Expect reverse proxy in front of this app
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context =>
            //     {
            //         await context.Response.WriteAsync("Hello World!");
            //     });
            // });
        }
    }
}
