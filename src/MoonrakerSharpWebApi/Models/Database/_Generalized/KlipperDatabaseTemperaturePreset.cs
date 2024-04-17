using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperDatabaseTemperaturePreset
    {
        #region Properties
        public Guid Id { get; set; } = Guid.Empty;

        public string Name { get; set; } = string.Empty;

        public string Gcode { get; set; } = string.Empty;

        public Dictionary<string, KlipperDatabaseTemperaturePresetHeater> Values { get; set; } = [];
        //public List<KlipperDatabaseTemperaturePresetHeater> Values { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
