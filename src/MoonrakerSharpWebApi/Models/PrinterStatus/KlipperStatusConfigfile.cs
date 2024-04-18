using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusConfigfile : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("warnings")]
        List<object> warnings = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("config")]
        KlipperConfig? config;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("settings")]
        KlipperSettings? settings;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("save_config_pending")]
        bool saveConfigPending;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
