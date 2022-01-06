using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperDatabaseTemperaturePreset
    {
        #region Properties
        public Guid Id { get; set; } = Guid.Empty;

        public string Name { get; set; }

        public string Gcode { get; set; }

        public Dictionary<string, KlipperDatabaseTemperaturePresetHeater> Values { get; set; } = new();
        //public List<KlipperDatabaseTemperaturePresetHeater> Values { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
