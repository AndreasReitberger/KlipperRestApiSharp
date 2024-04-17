using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValuePreset : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("id")]
        Guid id = Guid.Empty;

        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("gcode")]
        string gcode = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("values")]
        Dictionary<string, KlipperDatabaseFluiddHeaterElement> values = [];
        //public KlipperDatabaseMainsailValuePresetValues Values { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
