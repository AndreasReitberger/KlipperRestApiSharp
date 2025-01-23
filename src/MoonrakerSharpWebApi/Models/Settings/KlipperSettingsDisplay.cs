using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingsDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("down_pin")]
        public partial string DownPin { get; set; } = string.Empty;

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

        [JsonProperty("hd44780_protocol_init")]
        public partial bool Hd44780ProtocolInit { get; set; }

        [ObservableProperty]

        [JsonProperty("analog_range_up_pin")]
        public partial List<long> AnalogRangeUpPin { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("analog_range_click_pin")]
        public partial List<long> AnalogRangeClickPin { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("rs_pin")]
        public partial string RsPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("menu_root")]
        public partial string MenuRoot { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("menu_reverse_navigation")]
        public partial bool MenuReverseNavigation { get; set; }

        [ObservableProperty]

        [JsonProperty("analog_range_back_pin")]
        public partial List<long> AnalogRangeBackPin { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("d5_pin")]
        public partial string D5Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("encoder_steps_per_detent")]
        public partial long EncoderStepsPerDetent { get; set; }

        [ObservableProperty]

        [JsonProperty("encoder_fast_rate")]
        public partial double EncoderFastRate { get; set; }

        [ObservableProperty]

        [JsonProperty("up_pin")]
        public partial string UpPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("d7_pin")]
        public partial string D7Pin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_pullup_resistor")]
        public partial long AnalogPullupResistor { get; set; }

        [ObservableProperty]

        [JsonProperty("menu_timeout")]
        public partial long MenuTimeout { get; set; }

        [ObservableProperty]

        [JsonProperty("e_pin")]
        public partial string EPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("back_pin")]
        public partial string BackPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("display_group")]
        public partial string DisplayGroup { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("analog_range_down_pin")]
        public partial List<long> AnalogRangeDownPin { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("line_length")]
        public partial long LineLength { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
