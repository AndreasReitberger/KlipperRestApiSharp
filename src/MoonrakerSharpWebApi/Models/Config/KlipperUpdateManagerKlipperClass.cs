using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerKlipperClass : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type")]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("venv_args")]
        string venvArgs = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("is_system_service")]
        bool isSystemService;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("moved_origin")]
        Uri? movedOrigin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("origin")]
        Uri? origin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("primary_branch")]
        string primaryBranch = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_node_updates")]
        bool enableNodeUpdates;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("requirements")]
        string requirements = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("install_script")]
        string installScript = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
