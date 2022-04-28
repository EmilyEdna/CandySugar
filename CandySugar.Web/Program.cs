using Furion;
using Newtonsoft.Json.Serialization;
using SDKCore;

namespace CandySugar.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication.CreateBuilder(args).Inject().Build().Run();
        }
    }

    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).AddInjectWithUnifyResult();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            License.Register(new LicenseModel
            {
                 Account="EmilyEdna",
                 PassWord=DateTime.Now.ToString("yyyyMMdd")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}