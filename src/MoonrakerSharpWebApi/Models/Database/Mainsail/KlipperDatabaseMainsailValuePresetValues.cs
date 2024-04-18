using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperDatabaseMainsailHeaterElement? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder1")]
        KlipperDatabaseMainsailHeaterElement? extruder1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_bed")]
        KlipperDatabaseMainsailHeaterElement? heaterBed;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
