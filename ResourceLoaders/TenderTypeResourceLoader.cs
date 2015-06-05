using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class TenderTypeResourceLoader : ResourceLoader
    {
        public TenderTypeResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string tenderTypeId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{TenderType.Url}{(tenderTypeId == null ? "" : $"/{tenderTypeId}")}";
        public async Task<TenderType.Array> ListAsync(string locationId) => await io.GetAsyncWithException<TenderType.Array>(BuildUrl(locationId));
        public async Task<TenderType> RetrieveAsync(string locationId, string tenderTypeId) => await io.GetAsyncWithException<TenderType>(BuildUrl(locationId, tenderTypeId));
    }
}