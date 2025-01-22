using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueHeightmapSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("mesh")]
        public partial bool Mesh { get; set; }

        [ObservableProperty]
        
        [JsonProperty("scaleVisualMap")]
        public partial bool ScaleVisualMap { get; set; }

        [ObservableProperty]
        
        [JsonProperty("probed")]
        public partial bool Probed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("flat")]
        public partial bool Flat { get; set; }

        [ObservableProperty]
        
        [JsonProperty("wireframe")]
        public partial bool Wireframe { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
