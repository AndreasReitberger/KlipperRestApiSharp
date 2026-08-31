using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueHeightmapSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("mesh")]
        public partial bool Mesh { get; set; }

        [ObservableProperty]
        [JsonPropertyName("scaleVisualMap")]
        public partial bool ScaleVisualMap { get; set; }

        [ObservableProperty]
        [JsonPropertyName("probed")]
        public partial bool Probed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("flat")]
        public partial bool Flat { get; set; }

        [ObservableProperty]
        [JsonPropertyName("wireframe")]
        public partial bool Wireframe { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
