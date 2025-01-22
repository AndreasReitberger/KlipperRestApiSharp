using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePreset : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("id")]
        public partial Guid Id { get; set; } = Guid.Empty;

        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("gcode")]
        public partial string Gcode { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("values")]
        public partial Dictionary<string, KlipperDatabaseFluiddHeaterElement> Values { get; set; } = [];

        //public KlipperDatabaseMainsailValuePresetValues Values { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
