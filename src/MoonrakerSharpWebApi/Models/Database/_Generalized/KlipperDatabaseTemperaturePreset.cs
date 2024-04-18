using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseTemperaturePreset : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        Guid id = Guid.Empty;

        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        string gcode = string.Empty;

        [ObservableProperty]
        Dictionary<string, KlipperDatabaseTemperaturePresetHeater> values = [];
        //public List<KlipperDatabaseTemperaturePresetHeater> Values  = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
