using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueHeightmapSettings
    {
        #region Properties
        [JsonProperty("mesh")]
        public bool Mesh { get; set; }

        [JsonProperty("scaleVisualMap")]
        public bool ScaleVisualMap { get; set; }

        [JsonProperty("probed")]
        public bool Probed { get; set; }

        [JsonProperty("flat")]
        public bool Flat { get; set; }

        [JsonProperty("wireframe")]
        public bool Wireframe { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
