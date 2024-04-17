using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPlugin
    {
        #region Properties
        [JsonProperty("align_inline_thumbnail")]
        public bool AlignInlineThumbnail { get; set; }

        [JsonProperty("inline_thumbnail")]
        public bool InlineThumbnail { get; set; }

        [JsonProperty("inline_thumbnail_align_value")]
        public string InlineThumbnailAlignValue { get; set; } = string.Empty;

        [JsonProperty("inline_thumbnail_scale_value")]
        public long InlineThumbnailScaleValue { get; set; }

        [JsonProperty("installed")]
        public bool Installed { get; set; }

        [JsonProperty("installed_version")]
        public string InstalledVersion { get; set; } = string.Empty;

        [JsonProperty("scale_inline_thumbnail")]
        public bool ScaleInlineThumbnail { get; set; }

        [JsonProperty("state_panel_thumbnail")]
        public bool StatePanelThumbnail { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
