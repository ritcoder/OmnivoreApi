using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class ModifierResourceLoader : ResourceLoader
    {
        public ModifierResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string modifierId = null)
            => $"{MenuResourceLoader.BuildUrl(locationId)}/{Modifier.Url}{(modifierId == null ? "" : $"/{modifierId}")}";
        public async Task<Modifier.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Modifier.Array>(BuildUrl(locationId));
        public async Task<Modifier> RetrieveAsync(string locationId, string modifierId) => await io.GetAsyncWithException<Modifier>(BuildUrl(locationId, modifierId));
    }
}