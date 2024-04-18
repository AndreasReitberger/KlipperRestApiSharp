using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperDatabaseFluiddHeaterElement? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder1")]
        KlipperDatabaseFluiddHeaterElement? extruder1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_bed")]
        KlipperDatabaseFluiddHeaterElement? heaterBed;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
