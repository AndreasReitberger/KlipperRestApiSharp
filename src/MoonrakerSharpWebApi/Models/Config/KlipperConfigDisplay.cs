using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigDisplay
    {
        #region Properties
        [JsonProperty("down_pin")]
        public string DownPin { get; set; } = string.Empty;

        [JsonProperty("analog_range_back_pin")]
        public string AnalogRangeBackPin { get; set; } = string.Empty;

        [JsonProperty("click_pin")]
        public string ClickPin { get; set; } = string.Empty;

        [JsonProperty("d6_pin")]
        public string D6Pin { get; set; } = string.Empty;

        [JsonProperty("d4_pin")]
        public string D4Pin { get; set; } = string.Empty;

        [JsonProperty("lcd_type")]
        public string LcdType { get; set; } = string.Empty;

        [JsonProperty("d5_pin")]
        public string D5Pin { get; set; } = string.Empty;

        [JsonProperty("e_pin")]
        public string EPin { get; set; } = string.Empty;

        [JsonProperty("back_pin")]
        public string BackPin { get; set; } = string.Empty;

        [JsonProperty("rs_pin")]
        public string RsPin { get; set; } = string.Empty;

        [JsonProperty("analog_range_down_pin")]
        public string AnalogRangeDownPin { get; set; } = string.Empty;

        [JsonProperty("analog_range_up_pin")]
        public string AnalogRangeUpPin { get; set; } = string.Empty;

        [JsonProperty("up_pin")]
        public string UpPin { get; set; } = string.Empty;

        [JsonProperty("d7_pin")]
        public string D7Pin { get; set; } = string.Empty;

        [JsonProperty("analog_range_click_pin")]
        public string AnalogRangeClickPin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
