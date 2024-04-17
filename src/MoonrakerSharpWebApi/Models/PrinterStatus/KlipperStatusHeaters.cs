using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusHeaters
    {
        #region Properties
        [JsonProperty("available_sensors")]
        public List<string> AvailableSensors { get; set; } = new();

        [JsonProperty("available_heaters")]
        public List<string> AvailableHeaters { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
