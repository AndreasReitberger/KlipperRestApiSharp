using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFilamentSensorsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("status")]
        Dictionary<string, KlipperStatusFilamentSensor> status = [];

        [ObservableProperty]
        [property: JsonProperty("eventtime")]
        double? eventtime;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
