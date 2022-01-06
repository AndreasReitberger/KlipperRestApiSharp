using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseFluiddValuePreset
    {
        #region Properties
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gcode")]
        public string Gcode { get; set; }

        [JsonProperty("values")]
        public Dictionary<string, KlipperDatabaseFluiddHeaterElement> Values { get; set; } = new();
        //public KlipperDatabaseMainsailValuePresetValues Values { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
