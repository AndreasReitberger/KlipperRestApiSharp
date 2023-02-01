using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePresetValues
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperDatabaseFluiddHeaterElement Extruder { get; set; }
        [JsonProperty("extruder1")]
        public KlipperDatabaseFluiddHeaterElement Extruder1 { get; set; }

        [JsonProperty("heater_bed")]
        public KlipperDatabaseFluiddHeaterElement HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
