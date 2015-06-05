using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class LocationResourceLoader: ResourceLoader
    {
        public LocationResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId=null) => locationId==null ? Location.Url : $"{Location.Url}/{locationId}";

        public async Task<Location.Array> ListAsync() => await io.GetAsyncWithException<Location.Array>(BuildUrl());
        public async Task<Location> RetrieveAsync(string locationId)=> await io.GetAsyncWithException<Location>(BuildUrl(locationId));
    }
}