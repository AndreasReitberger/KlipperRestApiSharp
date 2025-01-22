using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusConfigfile : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("warnings")]
        public partial List<object> Warnings { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("config")]
        public partial KlipperConfig? Config { get; set; }

        [ObservableProperty]
        
        [JsonProperty("settings")]
        public partial KlipperSettings? Settings { get; set; }

        [ObservableProperty]
        
        [JsonProperty("save_config_pending")]
        public partial bool SaveConfigPending { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
