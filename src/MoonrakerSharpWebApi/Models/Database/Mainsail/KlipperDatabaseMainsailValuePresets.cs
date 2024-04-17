using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresets : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("presets")]
        Dictionary<Guid, KlipperDatabaseMainsailValuePreset> presets = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
