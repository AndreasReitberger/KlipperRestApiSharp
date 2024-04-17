using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebcamConfigResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("webcams")]
        List<KlipperDatabaseWebcamConfig> webcams = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        #endregion
    }
}
