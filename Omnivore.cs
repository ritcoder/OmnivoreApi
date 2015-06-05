using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmnivoreApi.ResourceHandlers;

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

        public static Omnivore io { get; } = new Omnivore {baseUrl = "http://api.omnivore.io", version = "0.1"};

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
            var finalUrl = buildUrl ? $"{baseUrl}/{version}/locations/{url}" : url;
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

    namespace ResourceHandlers
    {
        public class ResourceLoader
        {
            public ResourceLoader(Omnivore io)
            {
                this.io = io;
            }

            protected Omnivore io { get; }
        }
        public class LocationResourceLoader: ResourceLoader
        {
            public LocationResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId=null) => locationId==null ? Location.Url : $"{Location.Url}/{locationId}";

            public async Task<Location.Array> ListAsync() => await io.GetAsyncWithException<Location.Array>(BuildUrl());
            public async Task<Location> RetrieveAsync(string locationId)=> await io.GetAsyncWithException<Location>(BuildUrl(locationId));
        }
        public class TableResourceLoader : ResourceLoader
        {
            public TableResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string tableId = null)
                => $"{LocationResourceLoader.BuildUrl(locationId)}/{Table.Url}{(tableId == null ? "" : $"/{tableId}")}";
            public async Task<Table.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Table.Array>(BuildUrl(locationId));
            public async Task<Table> RetrieveAsync(string locationId, string tableId) => await io.GetAsyncWithException<Table>(BuildUrl(locationId,tableId));
        }
        public class EmployeeResourceLoader : ResourceLoader
        {
            public EmployeeResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string employeeId = null)
                => $"{LocationResourceLoader.BuildUrl(locationId)}/{Employee.Url}{(employeeId == null ? "" : $"/{employeeId}")}";
            public async Task<Employee.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Employee.Array>(BuildUrl(locationId));
            public async Task<Employee> RetrieveAsync(string locationId, string employeeId) => await io.GetAsyncWithException<Employee>(BuildUrl(locationId,employeeId));
        }
        public class OrderTypeResourceLoader : ResourceLoader
        {
            public OrderTypeResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string orderTypeId = null)
                => $"{LocationResourceLoader.BuildUrl(locationId)}/{OrderType.Url}{(orderTypeId == null ? "" : $"/{orderTypeId}")}";
            public async Task<OrderType.Array> ListAsync(string locationId) => await io.GetAsyncWithException<OrderType.Array>(BuildUrl(locationId));
            public async Task<OrderType> RetrieveAsync(string locationId, string orderTypeId) => await io.GetAsyncWithException<OrderType>(BuildUrl(locationId, orderTypeId));
        }
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
        public class PaymentResourceLoader : ResourceLoader
        {
            public PaymentResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string ticketId, string paymentId = null)
                => $"{TicketResourceLoader.BuildUrl(locationId,ticketId)}/{Payment.Url}{(paymentId == null ? "" : $"/{paymentId}")}";
            public async Task<Payment.Array> ListAsync(string locationId, string ticketId) 
                => await io.GetAsyncWithException<Payment.Array>(BuildUrl(locationId, ticketId));
            public async Task<Payment> RetrieveAsync(string locationId, string ticketId, string paymentId) 
                => await io.GetAsyncWithException<Payment>(BuildUrl(locationId, ticketId, paymentId));
            public async Task<Payment.PaymentStatus> MakeCardNotPresentPayment(string locationId, string ticketId, Payment.CardNotPresentPaymentParameters args)
                => await io.PostAsyncWithException<Payment.CardNotPresentPaymentParameters,Payment.PaymentStatus>(BuildUrl(locationId,ticketId),args);
            public async Task<Payment.PaymentStatus> MakeCardPresentPayment(string locationId, string ticketId, Payment.CardPresentPaymentParameters args)
                => await io.PostAsyncWithException<Payment.CardPresentPaymentParameters,Payment.PaymentStatus>(BuildUrl(locationId,ticketId),args);
            public async Task<Payment.PaymentStatus> MakeThirdPartyPayment(string locationId, string ticketId, Payment.ThirdPartyPaymentParameters args)
                => await io.PostAsyncWithException<Payment.ThirdPartyPaymentParameters,Payment.PaymentStatus>(BuildUrl(locationId,ticketId),args);
            public async Task<Payment.PaymentStatus> MakeGiftCardPayment(string locationId, string ticketId, Payment.GiftCardPaymentParameters args)
                => await io.PostAsyncWithException<Payment.GiftCardPaymentParameters,Payment.PaymentStatus>(BuildUrl(locationId,ticketId),args);
        }
        public class MenuResourceLoader : ResourceLoader
        {
            public MenuResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId) => $"{LocationResourceLoader.BuildUrl(locationId)}/{Menu.Url}";
            public async Task<Menu> GetAsync(string locationId) => await io.GetAsyncWithException<Menu>(BuildUrl(locationId));
        }
        public class CategoryResourceLoader : ResourceLoader
        {
            public CategoryResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string categoryId = null)
                => $"{MenuResourceLoader.BuildUrl(locationId)}/{Category.Url}{(categoryId == null ? "" : $"/{categoryId}")}";
            public async Task<Category.Array> ListAsync(string locationId) => await io.GetAsyncWithException<Category.Array>(BuildUrl(locationId));
            public async Task<Category> RetrieveAsync(string locationId, string categoryId) => await io.GetAsyncWithException<Category>(BuildUrl(locationId, categoryId));
        }
        public class MenuItemResourceLoader : ResourceLoader
        {
            public MenuItemResourceLoader(Omnivore io) : base(io)
            {
            }

            public static string BuildUrl(string locationId, string menuItemId = null)
                => $"{MenuResourceLoader.BuildUrl(locationId)}/{MenuItem.Url}{(menuItemId == null ? "" : $"/{menuItemId}")}";
            public async Task<MenuItem.Array> ListAsync(string locationId) => await io.GetAsyncWithException<MenuItem.Array>(BuildUrl(locationId));
            public async Task<MenuItem> RetrieveAsync(string locationId, string menuItemId) => await io.GetAsyncWithException<MenuItem>(BuildUrl(locationId, menuItemId));
        }
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
}