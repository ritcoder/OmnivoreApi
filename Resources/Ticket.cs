using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for restaurant Tickets, also known as an order. Tickets are a sub-resource of Location.
    /// Creating a Ticket is synonymous with opening an order to be fulfilled by a restaurant.
    /// Tickets can be created, modified, replaced, and deleted.
    /// </summary>
    public class Ticket : IResource
    {
        public const string Url = "tickets";

        /// <summary>
        /// The ticket’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// When false, items aren’t sent to kitchen until ticket is paid in full.
        /// </summary>
        [JsonProperty("auto_send")]
        public bool autoSend { get; set; }

        /// <summary>
        /// Timestamp when the Ticket was closed out. Null when ‘open’ is true
        /// </summary>
        [JsonProperty("closed_at")]
        public long? closedAt { get; set; }

        /// <summary>
        /// Number of guests on the Ticket
        /// </summary>
        [JsonProperty("guest_count")]
        public int guestCount { get; set; }

        /// <summary>
        /// Alphanumeric identifier for the Ticket. Note: unique amongst open Tickets
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Whether or not the Ticket has a remaining balance
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// Timestamp when the ticket was opened
        /// </summary>
        [JsonProperty("opened_at")]
        public long openedAt { get; set; }

        /// <summary>
        /// The number printed on the receipt
        /// </summary>
        [JsonProperty("ticket_number")]
        public long ticketNumber { get; set; }

        public Total totals { get; set; }
        [JsonProperty("void")]
        public bool isVoid { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public DateTime dateOpened => openedAt.ToDateTime();
        public DateTime? dateClosed => closedAt?.ToDateTime();

        public Employee employee => embedded?.employee;
        public List<TicketItem> items => embedded?.items;
        public OrderType orderType => embedded?.orderType;
        public List<Payment> payments => embedded?.payments;
        public RevenueCenter revenueCenter => embedded?.revenueCenter;
        public Table table => embedded?.table;

        public class Embedded
        {
            public Employee employee { get; set; }
            public List<TicketItem> items { get; set; }

            [JsonProperty("order_type")]
            public OrderType orderType { get; set; }

            public List<Payment> payments { get; set; }

            [JsonProperty("revenue_center")]
            public RevenueCenter revenueCenter { get; set; }

            public Table table { get; set; }
        }

        public class Links
        {
            public ArrayReference<TicketItem> items { get; set; }
            public ArrayReference<Payment> payments { get; set; }
            public SelfReference<Ticket> self { get; set; }
        }

        public class Array
        {
            [JsonProperty("_embedded")]
            public Store store { get; set; }

            public List<Ticket> items => store.tickets;

            public class Store
            {
                public List<Ticket> tickets { get; set; }
            }
        }

        public class OpenParameters
        {
            public string employee { get; set; }

            [JsonProperty("order_type")]
            public string orderType { get; set; }

            [JsonProperty("revenue_center")]
            public string revenueCenter { get; set; }

            public string table { get; set; }

            /// <summary>
            /// Number of guests involved with the Ticket
            /// </summary>
            [JsonProperty("guest_count")]
            public int guestCount { get; set; }

            /// <summary>
            /// The name for the ticket, to be displayed on the POS interface. Some POS systems will ignore this
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// When false, items aren’t sent to kitchen until ticket is paid in full
            /// </summary>
            [JsonProperty("auto_send")]
            public bool autoSend { get; set; }
        }

        public class Total
        {
            /// <summary>
            /// Remaining balance
            /// </summary>
            public int due { get; set; }

            /// <summary>
            /// Total from miscellaneous charges
            /// </summary>
            [JsonProperty("other_charges")]
            public int otherCharges { get; set; }

            /// <summary>
            /// Other fees such automatic gratuity or delivery fee
            /// </summary>
            [JsonProperty("service_charges")]
            public int serviceCharges { get; set; }

            /// <summary>
            /// Total cost of all Ticket Items
            /// </summary>
            [JsonProperty("sub_total")]
            public int subTotal { get; set; }

            /// <summary>
            /// Legislation imposed charges
            /// </summary>
            public int tax { get; set; }

            /// <summary>
            /// Total amount for the Ticket
            /// </summary>
            public int total { get; set; }
        }
    }
}