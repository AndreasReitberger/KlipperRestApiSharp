using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperActionListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("objects")]
        public partial List<string> Objects { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
