using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMesh : ObservableObject
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mesh_max")]
        List<double> meshMax = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mesh_matrix")]
        List<List<double>> meshMatrix = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("profile_name")]
        string profileName = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mesh_min")]
        List<double> meshMin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("probed_matrix")]
        List<List<double>> probedMatrix = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
