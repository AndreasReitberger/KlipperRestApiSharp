using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("down_pin")]
        public partial string DownPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_range_back_pin")]
        public partial string AnalogRangeBackPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("click_pin")]
        public partial string ClickPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("d6_pin")]
        public partial string D6Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("d4_pin")]
        public partial string D4Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("lcd_type")]
        public partial string LcdType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("d5_pin")]
        public partial string D5Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("e_pin")]
        public partial string EPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("back_pin")]
        public partial string BackPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("rs_pin")]
        public partial string RsPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_range_down_pin")]
        public partial string AnalogRangeDownPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_range_up_pin")]
        public partial string AnalogRangeUpPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("up_pin")]
        public partial string UpPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("d7_pin")]
        public partial string D7Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_range_click_pin")]
        public partial string AnalogRangeClickPin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
