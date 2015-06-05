using Newtonsoft.Json;
using OmnivoreApi.Utils;

namespace OmnivoreApi.Resources
{
    /// <summary>
    /// Resource for a restaurant Menu. A Menu has no intrinsic data, but instead contains other resources that describe the Menu.
    /// A Location only has one Menu
    /// </summary>
    public class Menu
    {
        public const string Url = "menu";

        [JsonProperty("_links")]
        public Links links { get; set; }

        public class Links : SelfLink<Menu>
        {
            public ArrayReference<Category> categories { get; set; }
            public ArrayReference<MenuItem> items { get; set; }
            public ArrayReference<Modifier> modifiers { get; set; }
        }
    }
}