using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerKlipperClass : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("type")]
        string type = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("venv_args")]
        string venvArgs = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("is_system_service")]
        bool isSystemService;

        [ObservableProperty]
        [property: JsonProperty("moved_origin")]
        Uri? movedOrigin;

        [ObservableProperty]
        [property: JsonProperty("origin")]
        Uri? origin;

        [ObservableProperty]
        [property: JsonProperty("primary_branch")]
        string primaryBranch = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("enable_node_updates")]
        bool enableNodeUpdates;

        [ObservableProperty]
        [property: JsonProperty("requirements")]
        string requirements = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("install_script")]
        string installScript = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
