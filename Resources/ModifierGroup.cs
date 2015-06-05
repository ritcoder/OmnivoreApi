using System.Collections.Generic;
using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Sets of Modifiers that can be added to a Menu Item
    /// </summary>
    public class ModifierGroup : IResource
    {
        public const string Url = "modifier_groups";

        /// <summary>
        /// The modifier group’s id as stored in the POS. Sometimes a compound value derived from other data
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The name of the group as stored in the POS
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The minimum number of modifiers that may be selected from this group
        /// </summary>
        public int minimum { get; set; }

        /// <summary>
        /// The maximum number of modifiers that may be selected from this group
        /// </summary>
        public int maximum { get; set; }

        /// <summary>
        /// True if n modifiers, where maximum >= n >= minimum, in this modifier group have to be submitted as modifiers to a ticket item
        /// </summary>
        public bool required { get; set; }

        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

        public List<Modifier> modifiers => embedded?.modifiers;

        public class Embedded
        {
            public List<Modifier> modifiers { get; set; }
        }

        public class Links : SelfLink<ModifierGroup>
        {
            public ArrayReference<Modifier> modifiers { get; set; }
        }

        public class Array : ResourceArray
        {
            [JsonProperty("_embedded")]
            public Store embedded { get; set; }

            public List<ModifierGroup> items => embedded?.modifierGroups;

            public class Store
            {
                [JsonProperty("modifier_groups")]
                public List<ModifierGroup> modifierGroups { get; set; }
            }
        }
    }
}