using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPlugin : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("align_inline_thumbnail")]
        public partial bool AlignInlineThumbnail { get; set; }

        [ObservableProperty]
        
        [JsonProperty("inline_thumbnail")]
        public partial bool InlineThumbnail { get; set; }

        [ObservableProperty]
        
        [JsonProperty("inline_thumbnail_align_value")]
        public partial string InlineThumbnailAlignValue { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("inline_thumbnail_scale_value")]
        public partial long InlineThumbnailScaleValue { get; set; }

        [ObservableProperty]
        
        [JsonProperty("installed")]
        public partial bool Installed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("installed_version")]
        public partial string InstalledVersion { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("scale_inline_thumbnail")]
        public partial bool ScaleInlineThumbnail { get; set; }

        [ObservableProperty]
        
        [JsonProperty("state_panel_thumbnail")]
        public partial bool StatePanelThumbnail { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
