using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("configs")]
        List<KlipperDatabaseMainsailValueWebcamConfig> configs = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("boolNavi")]
        bool boolNavi;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
