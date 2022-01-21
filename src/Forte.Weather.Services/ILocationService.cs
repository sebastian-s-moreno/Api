﻿namespace Forte.Weather.Services
{
    public interface ILocationService
    {

        public LocationModel? GetRecommendedLocation(ActivityPreference activity);

        public List<LocationModel> GetLocations();

        public LocationModel? GetLocation(string id);

        public Task<bool> AddLocation(LocationModel location);

        public bool DeleteLocation(string id);

        public Task<bool> UpdateLocation(string id, LocationModel location);

        public Task<TimeSerie?> GetUpdatedDetails(string? id, double? longitude, double? latitude);
    }
}