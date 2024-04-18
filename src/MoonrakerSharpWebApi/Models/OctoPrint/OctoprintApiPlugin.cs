using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPlugin : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("align_inline_thumbnail")]
        bool alignInlineThumbnail;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("inline_thumbnail")]
        bool inlineThumbnail;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("inline_thumbnail_align_value")]
        string inlineThumbnailAlignValue = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("inline_thumbnail_scale_value")]
        long inlineThumbnailScaleValue;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("installed")]
        bool installed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("installed_version")]
        string installedVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("scale_inline_thumbnail")]
        bool scaleInlineThumbnail;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state_panel_thumbnail")]
        bool statePanelThumbnail;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
