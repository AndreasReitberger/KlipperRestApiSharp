using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("extruder")]
        public partial KlipperDatabaseMainsailHeaterElement? Extruder { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder1")]
        public partial KlipperDatabaseMainsailHeaterElement? Extruder1 { get; set; }

        [ObservableProperty]

        [JsonProperty("heater_bed")]
        public partial KlipperDatabaseMainsailHeaterElement? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
