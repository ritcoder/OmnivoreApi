using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class TicketItemModifierResourceLoader : ResourceLoader
    {
        public TicketItemModifierResourceLoader(Omnivore io) : base(io)
        {
        }
        public static string BuildUrl(string locationId, string ticketId, string ticketItemId, string ticketItemModifierId = null)
            => $"{TicketItemResourceLoader.BuildUrl(locationId, ticketId, ticketItemId)}/{TicketItemModifier.Url}{(ticketItemModifierId == null ? "" : $"/{ticketItemModifierId}")}";
        public async Task<TicketItemModifier.Array> ListAsync(string locationId, string ticketId, string ticketItemId)
            => await io.GetAsyncWithException<TicketItemModifier.Array>(BuildUrl(locationId, ticketId, ticketItemId));
        public async Task<TicketItemModifier> RetrieveAsync(string locationId, string ticketId, string ticketItemId)
            => await io.GetAsyncWithException<TicketItemModifier>(BuildUrl(locationId, ticketId, ticketItemId));
    }
}