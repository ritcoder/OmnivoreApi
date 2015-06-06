using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for purchasable Ticket Items on a Ticket. Ticket Items are a sub-resource of Ticket.
    /// Creating a Ticket Item resource effectively adds an item to the ticket, which should be fulfilled by the restaurant.
    /// After being created, neither Modifiers nor Items can be manipulated in any way
    /// </summary>
    public class TicketItem : IResource
    {
        public const string Url = "items";

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
        public int pricePerUnit { get; set; }

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

            public List<ItemModifier> modifiers { get; set; } = new List<ItemModifier>();

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
}