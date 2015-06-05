using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class ModifierGroupResourceLoader : ResourceLoader
    {
        public ModifierGroupResourceLoader(Omnivore io) : base(io)
        {
        }

        public static string BuildUrl(string locationId, string menuItemId, string modifierGroupId = null)
            => $"{MenuItemResourceLoader.BuildUrl(locationId,menuItemId)}/{ModifierGroup.Url}{(modifierGroupId == null ? "" : $"/{modifierGroupId}")}";
        public async Task<ModifierGroup.Array> ListAsync(string locationId, string menuItemId) 
            => await io.GetAsyncWithException<ModifierGroup.Array>(BuildUrl(locationId, menuItemId));
        public async Task<ModifierGroup> RetrieveAsync(string locationId, string menuItemId, string modifierGroupId) 
            => await io.GetAsyncWithException<ModifierGroup>(BuildUrl(locationId, menuItemId));
    }
}