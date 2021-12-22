using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseMainsailValuePresetValues
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperHeaterElement Extruder { get; set; }
        [JsonProperty("extruder1")]
        public KlipperHeaterElement Extruder1 { get; set; }

        [JsonProperty("heater_bed")]
        public KlipperHeaterElement HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
