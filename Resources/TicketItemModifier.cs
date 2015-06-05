using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for a Modifier of a Ticket Item. Modifiers are a sub-resource of Ticket Items.
    /// Creating a Modifier is done when the Item is added to the Ticket.
    /// After being created, neither Modifiers nor Items can be manipulated in any way
    /// </summary>
    public class TicketItemModifier : IResource
    {
        public const string Url = "modifiers";

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
}