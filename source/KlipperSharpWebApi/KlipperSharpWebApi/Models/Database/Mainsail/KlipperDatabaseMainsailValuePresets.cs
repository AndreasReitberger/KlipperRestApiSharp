using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseMainsailValuePresets
    {
        #region Properties
        [JsonProperty("presets")]
        public Dictionary<Guid, KlipperDatabaseMainsailValuePreset> Presets { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
