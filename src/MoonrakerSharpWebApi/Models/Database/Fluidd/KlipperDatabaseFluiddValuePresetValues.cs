using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("extruder")]
        public partial KlipperDatabaseFluiddHeaterElement? Extruder { get; set; }

        [ObservableProperty]
        [JsonPropertyName("extruder1")]
        public partial KlipperDatabaseFluiddHeaterElement? Extruder1 { get; set; }

        [ObservableProperty]
        [JsonPropertyName("heater_bed")]
        public partial KlipperDatabaseFluiddHeaterElement? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
