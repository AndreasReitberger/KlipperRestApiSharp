using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaters : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("available_sensors")]
        public partial List<string> AvailableSensors { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("available_heaters")]
        public partial List<string> AvailableHeaters { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
