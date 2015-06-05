using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmnivoreApi.Exceptions;
using OmnivoreApi.ResourceLoaders;
using OmnivoreApi.Utils;

namespace OmnivoreApi
{
    public class Omnivore
    {
        public Omnivore()
        {
            modifierGroups = new ModifierGroupResourceLoader(this);
            modifiers = new ModifierResourceLoader(this);
            menuItems = new MenuItemResourceLoader(this);
            categories = new CategoryResourceLoader(this);
            menu = new MenuResourceLoader(this);
            payments = new PaymentResourceLoader(this);
            ticketItemModifiers = new TicketItemModifierResourceLoader(this);
            ticketItems = new TicketItemResourceLoader(this);
            tickets = new TicketResourceLoader(this);
            revenueCenters = new RevenueCenterResourceLoader(this);
            tenderTypes = new TenderTypeResourceLoader(this);
            orderTypes = new OrderTypeResourceLoader(this);
            employees = new EmployeeResourceLoader(this);
            tables = new TableResourceLoader(this);
            locations = new LocationResourceLoader(this);
        }

        public static Omnivore io { get; } = new Omnivore {baseUrl = "https://api.omnivore.io", version = "0.1"};

        public string baseUrl { get; set; }
        public string version { get; set; }
        public SecureString apiKey { get; set; }

        //public async Task<Ticket> OpenTicket(string locationId, OpenTicketParameters requestData)
        //{
        //    var result = await ExecuteWithException<OpenTicketParameters, ListTicketResult>(HttpMethod.Post, $"{locationId}/tickets", requestData);
        //    if (result?.embedded?.tickets?.Any()==false) throw new OmnivoreApiException(new OmnivoreApiError {code=-2,date=DateTime.UtcNow,error="No ticket was returned"});
        //    return result.embedded.tickets.First();
        //}

        //public async Task<ListTicketResult> ListTickets(string locationId)
        //{
        //    return await ExecuteAsyncWithException<object,ListTicketResult>(HttpMethod.Get, $"{locationId}/tickets");
        //}

        //public async Task<Ticket> RetrieveTicket(string locationId, string ticketId)
        //{
        //    return await ExecuteWithException<Ticket>(HttpMethod.Get, $"{locationId}/tickets/{ticketId}");
        //}

        public async Task<T> GetAsyncWithException<T>(string url, bool buildUrl=true)
        {
            return await ExecuteAsyncWithException<object,T>(HttpMethod.Get, url, null, buildUrl);
        }

        public async Task<TOutput> PostAsyncWithException<TInput, TOutput>(string url, TInput input,
            bool buildUrl = true)
        {
            return await ExecuteAsyncWithException<TInput, TOutput>(HttpMethod.Post, url, input, buildUrl);
        }
        
        public async Task<TResponse> ExecuteAsyncWithException<TRequest, TResponse>(
            HttpMethod method, string url, TRequest input = default(TRequest), bool buildUrl=true)
        {
            var result = await Execute<object, TResponse>(method, url, input,buildUrl);
            if (result.ok) return result.success;
            throw result.error.ToException();
        }

        private async Task<OmnivoreApiResult<TResponse,OmnivoreApiError>> Execute<TRequest, TResponse>(HttpMethod method, string url, TRequest input = default(TRequest), bool buildUrl=true)
        {
            var finalUrl = buildUrl ? $"{baseUrl}/{version}/{url}" : url;
            var request = new HttpRequestMessage(method, finalUrl);
            if (input != null)
            {
                var content = new StringContent(JsonConvert.SerializeObject(input));
                request.Content = content;
            }
            request.Headers.Add("Api-Key", apiKey.ToPlainText());
            var http = new HttpClient();
            var success = default(TResponse);
            var error = default(OmnivoreApiError);
            try
            {
                var response = await http.SendAsync(request);
                var jsonString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    success = JsonConvert.DeserializeObject<TResponse>(jsonString);
                else
                {
                    error = JsonConvert.DeserializeObject<OmnivoreApiError>(jsonString);
                    error.raw = jsonString;
                }
            }
            catch (Exception exception)
            {
                //todo: add custom processing for the errors
                var innerMostException = exception;
                while (innerMostException.InnerException != null)
                    innerMostException = innerMostException.InnerException;
                error = new OmnivoreApiError {code = -1, date = DateTime.UtcNow, error = innerMostException.Message};
            }
            return new OmnivoreApiResult<TResponse, OmnivoreApiError> {error=error,success = success};
        }

        public LocationResourceLoader locations { get; }
        public TableResourceLoader tables { get; }
        public EmployeeResourceLoader employees { get; }
        public OrderTypeResourceLoader orderTypes { get; }
        public TenderTypeResourceLoader tenderTypes { get; }
        public RevenueCenterResourceLoader revenueCenters { get; }
        public TicketResourceLoader tickets { get; }
        public TicketItemResourceLoader ticketItems { get; }
        public TicketItemModifierResourceLoader ticketItemModifiers { get; }
        public PaymentResourceLoader payments { get; }
        public MenuResourceLoader menu { get; }
        public CategoryResourceLoader categories { get; }
        public MenuItemResourceLoader menuItems { get; }
        public ModifierResourceLoader modifiers { get; }
        public ModifierGroupResourceLoader modifierGroups { get; }
    }
}