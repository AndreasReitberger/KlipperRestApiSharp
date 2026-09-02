using System;
using System.Collections.Generic;

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
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValuePresets);
        #endregion
    }
}
