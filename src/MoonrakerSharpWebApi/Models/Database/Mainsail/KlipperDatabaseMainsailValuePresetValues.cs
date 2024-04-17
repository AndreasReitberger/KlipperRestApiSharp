using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresetValues
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperDatabaseMainsailHeaterElement? Extruder { get; set; }
       
        [JsonProperty("extruder1")]
        public KlipperDatabaseMainsailHeaterElement? Extruder1 { get; set; }

        [JsonProperty("heater_bed")]
        public KlipperDatabaseMainsailHeaterElement? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
