using Forte.Weather.DataAccess.Infrastructure;
using Forte.Weather.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Yr.Facade;

namespace Forte.Weather.Services.Infrastructure
{
    public static class ServiceInstaller
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddDataAccess();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IYrFacade, YrFacade>();
            services.AddTransient<IRecommendationService, RecommendationService>();
        }
    }
}