using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class TicketItemResourceLoader : ResourceLoader
    {
        public TicketItemResourceLoader(Omnivore io) : base(io)
        {
        }
        public static string BuildUrl(string locationId, string ticketId, string ticketItemId = null)
            => $"{TicketResourceLoader.BuildUrl(locationId,ticketId)}/{TicketItem.Url}{(ticketItemId == null ? "" : $"/{ticketItemId}")}";
        public async Task<TicketItem.Array> ListAsync(string locationId, string ticketId)
            => await io.GetAsyncWithException<TicketItem.Array>(BuildUrl(locationId, ticketId));
        public async Task<Ticket> AddAsync(string locationId, string ticketId, TicketItem.AddParameters args)
            => await io.PostAsyncWithException<TicketItem.AddParameters, Ticket>(BuildUrl(locationId, ticketId), args);
        public async Task<TicketItem> RetrieveAsync(string locationId, string ticketId, string ticketItemId)
            => await io.GetAsyncWithException<TicketItem>(BuildUrl(locationId, ticketId, ticketItemId));
        public async Task<Ticket> VoidAsync(string locationId, string ticketId, string ticketItemId)
            => await io.PostAsyncWithException<object, Ticket>(BuildUrl(locationId, ticketId, ticketItemId), null);
    }
}