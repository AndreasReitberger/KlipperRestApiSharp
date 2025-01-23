using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public partial string Text { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public partial Dictionary<string, bool> Flags { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
