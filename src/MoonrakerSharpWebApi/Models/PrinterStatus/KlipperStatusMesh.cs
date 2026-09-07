using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMesh : ObservableObject
    {
        #region Properties

        [ObservableProperty]

        [JsonPropertyName("mesh_max")]
        public partial List<double> MeshMax { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("mesh_matrix")]
        public partial List<List<double>> MeshMatrix { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("profile_name")]
        public partial string ProfileName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("mesh_min")]
        public partial List<double> MeshMin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("probed_matrix")]
        public partial List<List<double>> ProbedMatrix { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusMesh);
        #endregion
    }
}
