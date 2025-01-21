using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("extruder")]
        public partial KlipperDatabaseFluiddHeaterElement? Extruder { get; set; }

        [ObservableProperty]
        
        [JsonProperty("extruder1")]
        public partial KlipperDatabaseFluiddHeaterElement? Extruder1 { get; set; }

        [ObservableProperty]
        
        [JsonProperty("heater_bed")]
        public partial KlipperDatabaseFluiddHeaterElement? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
