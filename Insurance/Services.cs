using Microsoft.Extensions.Configuration;
using Insurance.BL.Util.Communication;
using Insurance.BL.Util.Settings;
using Insurance.Insurance.BL.insurance;
using Insurance.Insurance.BL.insurancedetail;
using Insurance.Insurance.BL.setup;
using Microsoft.Extensions.DependencyInjection;
using Insurance.BL.Auth;
using Insurance.BL.Domain.RawSQL;

namespace Insurance
{
    public class Services
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<ISetupBL, SetupBL>();
            services.AddScoped<IInsuranceBL, InsuranceBL>();
            services.AddScoped<IInsuranceDetailBL, InsuranceDetailBL>();
            services.AddScoped<IUsersBL, UsersBL>();
            services.AddScoped<SettingsManager, SettingsManager>();
            services.AddScoped<MailSender, MailSender>();
            services.AddScoped<RawSQLBL, RawSQLBL>();

        }
    }
}
