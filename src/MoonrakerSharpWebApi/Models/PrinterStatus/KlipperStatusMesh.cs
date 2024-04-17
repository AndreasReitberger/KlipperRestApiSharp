using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMesh
    {
        #region Properties
        [JsonProperty("mesh_max")]
        public List<double> MeshMax { get; set; } = [];

        [JsonProperty("mesh_matrix")]
        public List<List<double>> MeshMatrix { get; set; } = [];

        [JsonProperty("profile_name")]
        public string ProfileName { get; set; } = string.Empty;

        [JsonProperty("mesh_min")]
        public List<double> MeshMin { get; set; } = [];

        [JsonProperty("probed_matrix")]
        public List<List<double>> ProbedMatrix { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
