using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusIdleTimeout : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("state")]
        [field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial KlipperIdleStates State { get; set; }

        [ObservableProperty]

        [JsonProperty("printing_time")]
        public partial double? PrintingTime { get; set; }

        [ObservableProperty]

        public partial bool ValidState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
