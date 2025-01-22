using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("count")]
        public partial long Count { get; set; }

        [ObservableProperty]
        
        [JsonProperty("nozzleDiameter")]
        public partial double NozzleDiameter { get; set; }

        [ObservableProperty]
        
        [JsonProperty("offsets")]
        public partial long[][] Offsets { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("sharedNozzle")]
        public partial bool SharedNozzle { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
