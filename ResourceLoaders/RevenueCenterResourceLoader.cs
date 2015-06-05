using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceHandlers
{
    public class RevenueCenterResourceLoader : ResourceLoader
    {
        public RevenueCenterResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string revenueCenterId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{RevenueCenter.Url}{(revenueCenterId == null ? "" : $"/{revenueCenterId}")}";
        public async Task<RevenueCenter.Array> ListAsync(string locationId) 
            => await io.GetAsyncWithException<RevenueCenter.Array>(BuildUrl(locationId));
        public async Task<RevenueCenter> RetrieveAsync(string locationId, string revenueCenterId) 
            => await io.GetAsyncWithException<RevenueCenter>(BuildUrl(locationId, revenueCenterId));
    }
}