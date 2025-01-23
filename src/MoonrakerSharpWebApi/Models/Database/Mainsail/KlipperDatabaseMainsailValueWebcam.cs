using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("configs")]
        public partial List<KlipperDatabaseMainsailValueWebcamConfig> Configs { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("boolNavi")]
        public partial bool BoolNavi { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
