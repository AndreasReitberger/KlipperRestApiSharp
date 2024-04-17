using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPlugin : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("align_inline_thumbnail")]
        bool alignInlineThumbnail;

        [ObservableProperty]
        [property: JsonProperty("inline_thumbnail")]
        bool inlineThumbnail;

        [ObservableProperty]
        [property: JsonProperty("inline_thumbnail_align_value")]
        string inlineThumbnailAlignValue = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("inline_thumbnail_scale_value")]
        long inlineThumbnailScaleValue;

        [ObservableProperty]
        [property: JsonProperty("installed")]
        bool installed;

        [ObservableProperty]
        [property: JsonProperty("installed_version")]
        string installedVersion = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("scale_inline_thumbnail")]
        bool scaleInlineThumbnail;

        [ObservableProperty]
        [property: JsonProperty("state_panel_thumbnail")]
        bool statePanelThumbnail;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
