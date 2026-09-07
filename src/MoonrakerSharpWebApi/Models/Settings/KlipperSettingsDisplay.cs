using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingsDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("down_pin")]
        public partial string DownPin { get; set; } = string.Empty;

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

        [JsonPropertyName("hd44780_protocol_init")]
        public partial bool Hd44780ProtocolInit { get; set; }

        [ObservableProperty]

        [JsonPropertyName("analog_range_up_pin")]
        public partial List<long> AnalogRangeUpPin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("analog_range_click_pin")]
        public partial List<long> AnalogRangeClickPin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("rs_pin")]
        public partial string RsPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("menu_root")]
        public partial string MenuRoot { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("menu_reverse_navigation")]
        public partial bool MenuReverseNavigation { get; set; }

        [ObservableProperty]

        [JsonPropertyName("analog_range_back_pin")]
        public partial List<long> AnalogRangeBackPin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("d5_pin")]
        public partial string D5Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("encoder_steps_per_detent")]
        public partial long EncoderStepsPerDetent { get; set; }

        [ObservableProperty]

        [JsonPropertyName("encoder_fast_rate")]
        public partial double EncoderFastRate { get; set; }

        [ObservableProperty]

        [JsonPropertyName("up_pin")]
        public partial string UpPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("d7_pin")]
        public partial string D7Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("analog_pullup_resistor")]
        public partial long AnalogPullupResistor { get; set; }

        [ObservableProperty]

        [JsonPropertyName("menu_timeout")]
        public partial long MenuTimeout { get; set; }

        [ObservableProperty]

        [JsonPropertyName("e_pin")]
        public partial string EPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("back_pin")]
        public partial string BackPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("display_group")]
        public partial string DisplayGroup { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("analog_range_down_pin")]
        public partial List<long> AnalogRangeDownPin { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("line_length")]
        public partial long LineLength { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperSettingsDisplay);
        #endregion
    }
}
