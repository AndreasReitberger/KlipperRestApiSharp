namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPlugin : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("align_inline_thumbnail")]
        public partial bool AlignInlineThumbnail { get; set; }

        [ObservableProperty]

        [JsonPropertyName("inline_thumbnail")]
        public partial bool InlineThumbnail { get; set; }

        [ObservableProperty]

        [JsonPropertyName("inline_thumbnail_align_value")]
        public partial string InlineThumbnailAlignValue { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("inline_thumbnail_scale_value")]
        public partial long InlineThumbnailScaleValue { get; set; }

        [ObservableProperty]

        [JsonPropertyName("installed")]
        public partial bool Installed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("installed_version")]
        public partial string InstalledVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("scale_inline_thumbnail")]
        public partial bool ScaleInlineThumbnail { get; set; }

        [ObservableProperty]

        [JsonPropertyName("state_panel_thumbnail")]
        public partial bool StatePanelThumbnail { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.OctoprintApiPlugin);
        #endregion
    }
}
