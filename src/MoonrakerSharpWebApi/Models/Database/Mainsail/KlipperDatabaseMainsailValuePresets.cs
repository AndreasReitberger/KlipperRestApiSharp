using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresets : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("presets")]
        public partial Dictionary<Guid, KlipperDatabaseMainsailValuePreset> Presets { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
