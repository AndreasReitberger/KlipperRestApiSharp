using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMacroResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("status")]
        public partial Dictionary<string, KlipperGcodeMacro> Status { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("eventtime")]
        public partial double Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
