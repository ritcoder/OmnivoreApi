using System.Threading.Tasks;
using OmnivoreApi.Resources;

namespace OmnivoreApi.ResourceHandlers
{
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
}