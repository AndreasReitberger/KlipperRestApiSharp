using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingsDisplay
    {
        #region Properties
        [JsonProperty("down_pin")]
        public string DownPin { get; set; } = string.Empty;

        [JsonProperty("click_pin")]
        public string ClickPin { get; set; } = string.Empty;

        [JsonProperty("d6_pin")]
        public string D6Pin { get; set; } = string.Empty;

        [JsonProperty("d4_pin")]
        public string D4Pin { get; set; } = string.Empty;

        [JsonProperty("lcd_type")]
        public string LcdType { get; set; } = string.Empty;

        [JsonProperty("hd44780_protocol_init")]
        public bool Hd44780ProtocolInit { get; set; }

        [JsonProperty("analog_range_up_pin")]
        public List<long> AnalogRangeUpPin { get; set; } = [];

        [JsonProperty("analog_range_click_pin")]
        public List<long> AnalogRangeClickPin { get; set; } = [];

        [JsonProperty("rs_pin")]
        public string RsPin { get; set; } = string.Empty;

        [JsonProperty("menu_root")]
        public string MenuRoot { get; set; } = string.Empty;

        [JsonProperty("menu_reverse_navigation")]
        public bool MenuReverseNavigation { get; set; }

        [JsonProperty("analog_range_back_pin")]
        public List<long> AnalogRangeBackPin { get; set; } = [];

        [JsonProperty("d5_pin")]
        public string D5Pin { get; set; } = string.Empty;

        [JsonProperty("encoder_steps_per_detent")]
        public long EncoderStepsPerDetent { get; set; }

        [JsonProperty("encoder_fast_rate")]
        public double EncoderFastRate { get; set; }

        [JsonProperty("up_pin")]
        public string UpPin { get; set; } = string.Empty;

        [JsonProperty("d7_pin")]
        public string D7Pin { get; set; } = string.Empty;

        [JsonProperty("analog_pullup_resistor")]
        public long AnalogPullupResistor { get; set; }

        [JsonProperty("menu_timeout")]
        public long MenuTimeout { get; set; }

        [JsonProperty("e_pin")]
        public string EPin { get; set; } = string.Empty;

        [JsonProperty("back_pin")]
        public string BackPin { get; set; } = string.Empty;

        [JsonProperty("display_group")]
        public string DisplayGroup { get; set; } = string.Empty;

        [JsonProperty("analog_range_down_pin")]
        public List<long> AnalogRangeDownPin { get; set; } = [];

        [JsonProperty("line_length")]
        public long LineLength { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
