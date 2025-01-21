using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodesResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("gcode_store")]
        public partial List<KlipperGcode> Gcodes { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
