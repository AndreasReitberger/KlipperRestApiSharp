using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusConfigfile : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("warnings")]
        List<object> warnings = [];

        [ObservableProperty]
        [property: JsonProperty("config")]
        KlipperConfig? config;

        [ObservableProperty]
        [property: JsonProperty("settings")]
        KlipperSettings? settings;

        [ObservableProperty]
        [property: JsonProperty("save_config_pending")]
        bool saveConfigPending;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
