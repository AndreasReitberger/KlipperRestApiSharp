using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterState : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        string text = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        Dictionary<string, bool> flags = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
