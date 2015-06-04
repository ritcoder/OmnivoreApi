using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OmnivoreApi
{
    public class OmnivoreReference
    {
        public string href { get; set; }
        public string profile { get; set; }
    }

    public class OmnivoreSelfReference : OmnivoreReference
    {
        public string etag { get; set; }
    }

    public class OmnivoreSelfLink
    {
        public OmnivoreSelfReference self { get; set; }
    }

    public class OmnivoreApiError
    {
        public int code { get; set; }
        public string error { get; set; }
        public DateTime date { get; set; }
        public string raw { get; set; }

        public OmnivoreApiException ToException()
        {
            return new OmnivoreApiException(this);
        }
    }

    public class OmnivoreApiException: Exception
    {
        public OmnivoreApiException(OmnivoreApiError error)
        {
            details = error;
        }

        public OmnivoreApiError details { get;}
    }

    public class OmnivoreApiResult<TSuccess, TError>
    {
        public TSuccess success { get; set; }
        public TError error { get; set; }
        public bool ok => error == null;
        public TimeSpan? duration { get; set; }
    }

    public class OmnivoreQueryResult
    {
        public int? count { get; set; }
        [JsonProperty("_links")]
        public OmnivoreSelfLink links { get; set; }
    }
    public class ListTicketResult : OmnivoreQueryResult
    {
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public class Embedded
        {
            public List<Ticket> tickets { get; set; }
        }
    }
    public interface IOmnivoreRequestData
    {

    }
    public class OpenTicketParameters : IOmnivoreRequestData
    {
        public string employee { get; set; }
        [JsonProperty("order_type")]
        public string orderType { get; set; }
        [JsonProperty("revenue_center")]
        public string revenueCenter { get; set; }
        public string table { get; set; }
        [JsonProperty("guest_count")]
        public int guestCount { get; set; }
        public string name { get; set; }
        [JsonProperty("auto_send")]
        public bool autoSend { get; set; }
    }

    public class Ticket
    {
        [JsonProperty("auto_send")]
        public bool autoSend { get; set; }
        [JsonProperty("closed_at")]
        public long? closedAt { get; set; }
        public string id { get; set; }
        [JsonProperty("guest_count")]
        public int guestCount { get; set; }
        public string name { get; set; }
        public bool open { get; set; }
        [JsonProperty("opened_at")]
        public long openedAt { get; set; }
        [JsonProperty("ticket_number")]
        public long ticketNumber { get; set; }
        public Total totals { get; set; }
        [JsonProperty("_links")]
        public Links links { get; set; }
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public DateTime dateOpened => openedAt.ToDateTime();
        public DateTime? dateClosed => closedAt?.ToDateTime();

        public class Embedded
        {
            //todo: expand this
        }
        public class Links
        {
            public OmnivoreReference items { get; set; }
            public OmnivoreReference payments { get; set; }
            public OmnivoreSelfReference self { get; set; }
        }
        public class Total
        {
            public int due { get; set; }
            [JsonProperty("other_charges")]
            public int otherCharges { get; set; }
            [JsonProperty("service_charges")]
            public int serviceCharges { get; set; }
            [JsonProperty("sub_total")]
            public int subTotal { get; set; }
            public int tax { get; set; }
            public int total { get; set; }
        }
    }

    public class Omnivore
    {
        public static string baseUrl { get; set; } = "http://api.omnivore.io";
        public static string version { get; set; } = "0.1";
        public static SecureString apiKey { get; set; }

        public async Task<Ticket> OpenTicket(string locationId, OpenTicketParameters requestData)
        {
            var result = await ExecuteWithException<OpenTicketParameters, ListTicketResult>(HttpMethod.Post, $"{locationId}/tickets", requestData);
            if (result?.embedded?.tickets?.Any()==false) throw new OmnivoreApiException(new OmnivoreApiError {code=-2,date=DateTime.UtcNow,error="No ticket was returned"});
            return result.embedded.tickets.First();
        }

        public async Task<ListTicketResult> ListTickets(string locationId)
        {
            return await ExecuteWithException<ListTicketResult>(HttpMethod.Get, $"{locationId}/tickets");
        }

        public async Task<Ticket> RetrieveTicket(string locationId, string ticketId)
        {
            return await ExecuteWithException<Ticket>(HttpMethod.Get, $"{locationId}/tickets/{ticketId}");
        }

        private static async Task<TResponse> ExecuteWithException<TResponse>( HttpMethod method, string url)
        {
            var result = await Execute<object, TResponse>(method, url);
            if (result.ok) return result.success;
            throw result.error.ToException();
        }
        private static async Task<TResponse> ExecuteWithException<TRequest, TResponse>(
            HttpMethod method, string url, TRequest input = default(TRequest))
        {
            var result = await Execute<object, TResponse>(method, url, input);
            if (result.ok) return result.success;
            throw result.error.ToException();
        }

        private static async Task<OmnivoreApiResult<TResponse,OmnivoreApiError>> Execute<TRequest, TResponse>(HttpMethod method, string url, TRequest input = default(TRequest))
        {
            var finalUrl = $"{baseUrl}/{version}/locations/{url}";
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
    }

    public static class Extensions
    {
        public static SecureString AppendText(this SecureString target, string input, bool makeReadOnly = true)
        {
            if (!string.IsNullOrEmpty(input))
            {
                foreach (var c in input) target.AppendChar(c);
            }
            if (makeReadOnly) target.MakeReadOnly();
            return target;
        }

        public static string ToPlainText(this SecureString src)
        {
            var ptr = Marshal.SecureStringToBSTR(src);
            var output = Marshal.PtrToStringBSTR(ptr);
            Marshal.ZeroFreeBSTR(ptr);
            return output;
        }

        public static DateTime ToDateTime(this long seconds) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
    }
}
