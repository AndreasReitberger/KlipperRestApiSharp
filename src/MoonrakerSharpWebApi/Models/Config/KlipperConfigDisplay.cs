using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("down_pin")]
        public partial string DownPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("analog_range_back_pin")]
        public partial string AnalogRangeBackPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("click_pin")]
        public partial string ClickPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("d6_pin")]
        public partial string D6Pin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("d4_pin")]
        public partial string D4Pin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("lcd_type")]
        public partial string LcdType { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("d5_pin")]
        public partial string D5Pin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("e_pin")]
        public partial string EPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("back_pin")]
        public partial string BackPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("rs_pin")]
        public partial string RsPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("analog_range_down_pin")]
        public partial string AnalogRangeDownPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("analog_range_up_pin")]
        public partial string AnalogRangeUpPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("up_pin")]
        public partial string UpPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("d7_pin")]
        public partial string D7Pin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("analog_range_click_pin")]
        public partial string AnalogRangeClickPin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
