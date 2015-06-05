using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for ticket Payments, also known as an order. Payments are a sub-resource of Ticket.
    /// Payments can be made and listed.
    /// </summary>
    public class Payment : IResource
    {
        public const string Url = "payments";
        public const string CardPresent = "card_present";
        public const string CardNotPresent = "card_not_present";
        public const string ThirdParty = "3rd_party";
        public const string GiftCard = "gift_card";

        public string id { get; set; }

        /// <summary>
        /// The type of payment. One of: “card_not_present”, “3rd_party”, “gift_card”.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The amount paid, in cents, excluding tip
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// The amount, in cents, paid as tip
        /// </summary>
        public int tip { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Payment> items => embedded?.payments;

            public class Store
            {
                public List<Payment> payments { get; set; }
            }
        }

        public class PaymentParametersBase
        {
            public string type { get; set; }

            /// <summary>
            /// The amount, in cents, excluding tip, to be paid
            /// </summary>
            public int amount { get; set; }

            /// <summary>
            /// The amount, in cents, to be paid as tip
            /// </summary>
            public int tip { get; set; }
        }

        public class CardNotPresentPaymentParameters : PaymentParametersBase
        {
            public CardNotPresentPaymentParameters()
            {
                type = CardNotPresent;
            }

            [JsonProperty("card_info")]
            public CardInfo cardInfo { get; set; }

            public class CardInfo
            {
                /// <summary>
                /// The credit card number, as a string
                /// </summary>
                public string number { get; set; }

                /// <summary>
                /// The month the card expires
                /// </summary>
                [JsonProperty("exp_month")]
                public int expirationMonth { get; set; }

                /// <summary>
                /// The four-digit year the card expires
                /// </summary>
                [JsonProperty("exp_year")]
                public int expirationYear { get; set; }

                /// <summary>
                /// The cvc value usually found on the back of the card
                /// </summary>
                public int cvc2 { get; set; }
            }
        }

        public class ThirdPartyPaymentParameters : PaymentParametersBase
        {
            public ThirdPartyPaymentParameters()
            {
                type = ThirdParty;
            }

            /// <summary>
            /// The Tender Type ID to use
            /// </summary>
            [JsonProperty("tender_type")]
            public string tenderType { get; set; }

            /// <summary>
            /// A comment about the payment for record. (ex: external transaction id)
            /// </summary>
            [JsonProperty("payment_source")]
            public string paymentSource { get; set; }
        }

        public class GiftCardPaymentParameters : PaymentParametersBase
        {
            public GiftCardPaymentParameters()
            {
                type = GiftCard;
            }

            [JsonProperty("card_info")]
            public CardInfo cardInfo { get; set; }

            public class CardInfo
            {
                /// <summary>
                /// The gift card number, as a string
                /// </summary>
                public string number { get; set; }
            }
        }

        public class CardPresentPaymentParameters : PaymentParametersBase
        {
            public CardPresentPaymentParameters()
            {
                type = CardPresent;
            }

            [JsonProperty("card_info")]
            public CardInfo cardInfo { get; set; }

            public class CardInfo
            {
                /// <summary>
                /// The track 1 and/or track 2 data read from a card reader
                /// </summary>
                public string data { get; set; }
            }
        }

        /// <summary>
        /// Data returned after a payment
        /// </summary>
        public class PaymentStatus
        {
            [JsonProperty("amount_paid")]
            public int amountPaid { get; set; }

            public bool accepted { get; set; }

            [JsonProperty("ticket_closed")]
            public bool ticketClosed { get; set; }

            [JsonProperty("balance_remaining")]
            public int balanceRemaining { get; set; }

            public string type { get; set; }
            public Ticket ticket { get; set; }
        }
    }
}