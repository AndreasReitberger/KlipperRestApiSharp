using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseTemperaturePreset : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        public partial Guid Id { get; set; } = Guid.Empty;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Dictionary<string, KlipperDatabaseTemperaturePresetHeater> Values { get; set; } = [];

        //public List<KlipperDatabaseTemperaturePresetHeater> Values  = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
