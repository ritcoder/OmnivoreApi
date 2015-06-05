using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OmnivoreApi
{
    public class Reference<T>
    {
        public string href { get; set; }
        public string profile { get; set; }

        public async Task<T> GetAsync(Omnivore io)
        {
            return await io.GetAsyncWithException<T>(href, false);
        }
    }
    public class ArrayReference<T>: Reference<List<T>> { }

    public class SelfReference<T> : Reference<T>
    {
        public string etag { get; set; }
    }

    public class SelfLink<T>
    {
        public SelfReference<T> self { get; set; }
    }

    public class ResourceArray
    {
        public int count { get; set; }
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

    //public class OmnivoreQueryResult
    //{
    //    public int? count { get; set; }
    //    [JsonProperty("_links")]
    //    public SelfLink links { get; set; }
    //}

    //public class ListTicketResult : OmnivoreQueryResult
    //{
    //    [JsonProperty("_embedded")]
    //    public Embedded embedded { get; set; }

    //    public class Embedded
    //    {
    //        public List<Ticket> tickets { get; set; }
    //    }
    //}
    //public interface IOmnivoreRequestData
    //{

    //}

    

    public interface IResource
    {
        string id { get; set; }
    }

    /// <summary>
    /// A Location resource contains data about a restaurant location.
    /// Locations can be created, modified, replaced, and deleted.
    /// </summary>
    public class Location : IResource
    {
        public const string Url = "Locations";
        /// <summary>
        /// The location’s id within our database
        /// </summary>
        public string id { get; set; }

        public Address address { get; set; }

        /// <summary>
        /// The name of the Location
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 10 digits, no punctuation
        /// </summary>
        public string phone { get; set; }

        public string website { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

        public class Address
        {
            public string city { get; set; }

            /// <summary>
            /// 2 character code (e.g. “CA”)
            /// </summary>
            public string state { get; set; }

            public string street1 { get; set; }
            public string street2 { get; set; }
            public string zip { get; set; }
        }

        public class Links : SelfLink<Location>
        {
            public ArrayReference<Employee> employees { get; set; }
            public ArrayReference<Menu> menus { get; set; }
            [JsonProperty("order_types")]
            public ArrayReference<OrderType> orderTypes { get; set; }
            [JsonProperty("revenue_centers")]
            public ArrayReference<RevenueCenter> revenueCenters { get; set; }
            public ArrayReference<Table> tables { get; set; }
            [JsonProperty("tender_types")]
            public ArrayReference<TenderType> tenderTypes { get; set; }
            public ArrayReference<Ticket> tickets { get; set; }
        }

        public class Array: ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Location> items => embedded?.locations; 

            public class Store
            {
                public List<Location> locations { get; set; } 
            }
        }
    }

    /// <summary>
    /// Resource for a Table inside of a specific Location and Revenue Center
    /// </summary>
    public class Table : IResource
    {
        public const string Url = "Tables";
        /// <summary>
        /// The table’s id as stored in the POS
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Whether or not the table is ready for immediate use
        /// </summary>
        public bool available { get; set; }
        /// <summary>
        /// Alphanumeric identifier from the POS. Not guarenteed to be the same as the id
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Integer identifier from the POS. Not guarenteed to be the same as the id
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// Number of seats at the table
        /// </summary>
        public int seats { get; set; }
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }
        [JsonProperty("_links")]
        public SelfLink<Table> links { get; set; }
        /// <summary>
        /// Only contains Tickets with a remaining balance
        /// </summary>
        public List<Ticket> openedTickets => embedded?.openTickets;
        public RevenueCenter revenueCenter => embedded?.revenueCenter;

        public class Embedded
        {
            [JsonProperty("open_tickets")]
            public List<Ticket> openTickets { get; set; } 
            [JsonProperty("revenue_center")]
            public RevenueCenter revenueCenter { get; set; }
        }

        public class Array: ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Table> items => embedded?.tables; 

            public class Store
            {
                public List<Table> tables { get; set; }
            }
        }
    }

    /// <summary>
    /// Resource for a Employee inside of a specific Location.
    /// </summary>
    public class Employee : IResource
    {
        public const string Url = "Employees";
        /// <summary>
        /// The employee’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// The name of the employee as printed on a ticket
        /// </summary>
        [JsonProperty("check_name")]
        public string checkName { get; set; }
        /// <summary>
        /// The employee’s first name as stored in the POS
        /// </summary>
        [JsonProperty("first_name")]
        public string firstName { get; set; }
        /// <summary>
        /// The employee’s last name as stored in the POS
        /// </summary>
        [JsonProperty("last_name")]
        public string lastName { get; set; }
        /// <summary>
        /// The employee’s terminal login info.
        /// </summary>
        public string login { get; set; }

        [JsonProperty("_links")]
        public SelfLink<Employee> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<Employee> items => embedded?.employees;

            public class Store
            {
                public List<Employee> employees { get; set; }
            }
        }
    }

    /// <summary>
    /// Resource for an Order Type inside of a specific Location
    /// </summary>
    public class OrderType : IResource
    {
        public const string Url = "Order_Types";
        /// <summary>
        /// The order type’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Whether or not tickets can currently be created with this Order Type
        /// </summary>
        public bool available { get; set; }
        /// <summary>
        /// The name of the order type as stored in the POS
        /// </summary>
        public string name { get; set; }
        [JsonProperty("_links")]
        public SelfLink<OrderType> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<OrderType> items => embedded?.orderTypes; 

            public class Store
            {
                [JsonProperty("order_types")]
                public List<OrderType> orderTypes { get; set; }
            }
        } 
    }
    /// <summary>
    /// Resource for an accepted Tender Type inside of a specific Location
    /// </summary>
    public class TenderType: IResource
    {
        public const string Url = "Tender_Types";
        /// <summary>
        /// The tender type’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// The name of the Tender Type as stored in the POS
        /// </summary>
        public string name { get; set; }
        [JsonProperty("_links")]
        public SelfLink<TenderType> links { get; set; }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<TenderType> items => embedded?.tenderTypes;

            public class Store
            {
                [JsonProperty("tender_types")]
                public List<TenderType> tenderTypes { get; set; }
            }
        } 
    }

    /// <summary>
    /// A Revenue Center is a specified sub-section of a Location. For example, a Location might have both a bar and a dining area.
    /// Revenue Centers are divided differently at every Location
    /// </summary>
    public class RevenueCenter : IResource
    {
        public const string Url = "Revenue_Centers";
        /// <summary>
        /// The revenue center’s id as stored in the POS system
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Whether or not this is the default Revenue Center for new Tickets
        /// </summary>
        public bool @default { get; set; }
        /// <summary>
        /// The name of the Revenue Center as stored in the POS
        /// </summary>
        public string name { get; set; }
        [JsonProperty("_links")]
        public SelfLink<RevenueCenter> links { get; set; }
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }
        /// <summary>
        /// Only contains Tickets with a remaining balance
        /// </summary>
        public List<Ticket> openTickets => embedded?.openTickets;
        public List<Table> tables => embedded?.tables; 

        public class Embedded
        {
            /// <summary>
            /// Only contains Tickets with a remaining balance
            /// </summary>
            [JsonProperty("open_tickets")]
            public List<Ticket> openTickets { get; set; }
            public List<Table> tables { get; set; } 
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<RevenueCenter> items => embedded?.revenueCenters;

            public class Store
            {
                [JsonProperty("revenue_centers")]
                public List<RevenueCenter> revenueCenters { get; set; }
            }
        }
    }

    public class Ticket: IResource
    {
        public const string Url = "Tickets";
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
        public bool @void { get; set; }

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

    /// <summary>
    /// Resource for purchasable Ticket Items on a Ticket. Ticket Items are a sub-resource of Ticket.
    /// Creating a Ticket Item resource effectively adds an item to the ticket, which should be fulfilled by the restaurant.
    /// After being created, neither Modifiers nor Items can be manipulated in any way
    /// </summary>
    public class TicketItem: IResource
    {
        public const string Url = "Items";
        /// <summary>
        /// The ticket item’s id. Not necessarily related to the POS system’s storage model
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// A special comment manually placed on the ticket item
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// The name of the item as printed on a receipt
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The price per unit, in cents
        /// </summary>
        [JsonProperty("price_per_unit")]
        public string pricePerUnit { get; set; }
        /// <summary>
        /// Units ordered
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// True if item has been sent to the kitchen. False if item has been held
        /// </summary>
        public bool sent { get; set; }
        [JsonProperty("_links")]
        public Links links { get; set; }
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public List<Modifier> modifiers => embedded?.modifiers;

        public class Links : SelfLink<TicketItem>
        {
            [JsonProperty("menu_item")]
            public Reference<MenuItem> menuItem { get; set; }
            public ArrayReference<Modifier> modifiers { get; set; } 
        }

        public class Embedded
        {
            public List<Modifier> modifiers { get; set; } 
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<TicketItem> items => embedded?.items;

            public class Store
            {
                public List<TicketItem> items { get; set; } 
            }
        }

        public class AddParameters
        {
            /// <summary>
            /// A Menu Item ID
            /// </summary>
            [JsonProperty("menu_item")]
            public string menuItem { get; set; }
            /// <summary>
            /// Defaults to 1
            /// </summary>
            public int quantity { get; set; } = 1;
            /// <summary>
            /// The price level of the menu item being added. Defaults to “1”
            /// </summary>
            [JsonProperty("price_level")]
            public string priceLevel { get; set; } = "1";
            /// <summary>
            /// Any text, typically special instructions that aren’t available through a modifier
            /// </summary>
            public string comment { get; set; }
            public List<ItemModifier> modifiers { get; set; }

            public class ItemModifier
            {
                /// <summary>
                /// The modifier ID
                /// </summary>
                public string modifier { get; set; }
                /// <summary>
                /// The number of units, default 1
                /// </summary>
                public int quantity { get; set; } = 1;
                /// <summary>
                /// The price level of the modifier. Defaults to “1”
                /// </summary>
                [JsonProperty("price_level")]
                public string priceLevel { get; set; } = "1";
                /// <summary>
                /// Any text, typically special instructions that aren’t available through a modifier
                /// </summary>
                public string comment { get; set; }
            }
        }
    }
    /// <summary>
    /// Resource for a Modifier of a Ticket Item. Modifiers are a sub-resource of Ticket Items.
    /// Creating a Modifier is done when the Item is added to the Ticket.
    /// After being created, neither Modifiers nor Items can be manipulated in any way
    /// </summary>
    public class TicketItemModifier : IResource
    {
        public const string Url = "Modifiers";
        /// <summary>
        /// The ticket item modifier’s id. Not necessarily related to the POS system’s storage model
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// A special comment manually placed on the ticket item modifier
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// The name of the modifier as printed on a receipt
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The price per unit, in cents
        /// </summary>
        [JsonProperty("price_per_unit")]
        public string pricePerUnit { get; set; }
        /// <summary>
        /// Units ordered
        /// </summary>
        public int quantity { get; set; }
        [JsonProperty("_links")]
        public Links links { get; set; }
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        public Modifier modifier => embedded?.modifier;

        public class Links : SelfLink<TicketItemModifier>
        {
            public Reference<Modifier> modifier { get; set; } 
        }

        public class Embedded
        {
            public Modifier modifier { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<TicketItemModifier> items => embedded?.modifiers; 

            public class Store
            {
                public List<TicketItemModifier> modifiers { get; set; }
            }
        }
    }
    /// <summary>
    /// Resource for ticket Payments, also known as an order. Payments are a sub-resource of Ticket.
    /// Payments can be made and listed.
    /// </summary>
    public class Payment : IResource
    {
        public const string Url = "Payments";
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
    /// <summary>
    /// Resource for a restaurant Menu. A Menu has no intrinsic data, but instead contains other resources that describe the Menu.
    /// A Location only has one Menu
    /// </summary>
    public class Menu
    {
        public const string Url = "Menu";
        [JsonProperty("_links")]
        public Links links { get; set; }

        public class Links : SelfLink<Menu>
        {
            public ArrayReference<Category> categories { get; set; }
            public ArrayReference<MenuItem> items { get; set; }
            public ArrayReference<Modifier> modifiers { get; set; }   
        }
    }
    /// <summary>
    /// A Category on a Menu that contains Menu Items
    /// </summary>
    public class Category : IResource
    {
        public const string Url = "Categories";
        /// <summary>
        /// The menu category’s id as stored in the POS. Sometimes a compound value derived from other data
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// The name of the Category as stored in the POS
        /// </summary>
        public string name { get; set; }

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
