using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceLoaders
{
    public class TicketResourceLoader : ResourceLoader
    {
        public TicketResourceLoader(Omnivore io) : base(io)
        {
        }
        public static string BuildUrl(string locationId, string ticketId = null)
            => $"{LocationResourceLoader.BuildUrl(locationId)}/{Ticket.Url}{(ticketId == null ? "" : $"/{ticketId}")}";
        public async Task<Ticket.Array> ListAsync(string locationId)
            => await io.GetAsyncWithException<Ticket.Array>(BuildUrl(locationId));
        public async Task<Ticket> OpenAsync(string locationId,Ticket.OpenParameters args)
            => await io.PostAsyncWithException<Ticket.OpenParameters,Ticket>(BuildUrl(locationId),args);
        public async Task<Ticket> RetrieveAsync(string locationId, string ticketId)
            => await io.GetAsyncWithException<Ticket>(BuildUrl(locationId, ticketId));
        public async Task<Ticket> VoidAsync(string locationId, string ticketId, bool @void)
            => await io.PostAsyncWithException<bool,Ticket>(BuildUrl(locationId, ticketId), @void);
    }
}