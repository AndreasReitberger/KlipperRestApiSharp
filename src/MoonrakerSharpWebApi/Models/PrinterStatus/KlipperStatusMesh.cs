using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMesh : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        
        [JsonProperty("mesh_max")]
        public partial List<double> MeshMax { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("mesh_matrix")]
        public partial List<List<double>> MeshMatrix { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("profile_name")]
        public partial string ProfileName { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("mesh_min")]
        public partial List<double> MeshMin { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("probed_matrix")]
        public partial List<List<double>> ProbedMatrix { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
