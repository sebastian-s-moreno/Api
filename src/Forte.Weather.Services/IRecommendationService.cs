using Forte.Weather.Services.Models;

namespace Forte.Weather.Services
{
    public interface IRecommendationService
    {
        public Location? GetRecommendedLocation(Activity activity);
    }
}
