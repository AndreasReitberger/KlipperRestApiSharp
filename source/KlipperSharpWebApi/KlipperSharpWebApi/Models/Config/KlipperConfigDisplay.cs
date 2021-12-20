using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperConfigDisplay
    {
        #region Properties
        [JsonProperty("down_pin")]
        public string DownPin { get; set; }

        [JsonProperty("analog_range_back_pin")]
        public string AnalogRangeBackPin { get; set; }

        [JsonProperty("click_pin")]
        public string ClickPin { get; set; }

        [JsonProperty("d6_pin")]
        public string D6Pin { get; set; }

        [JsonProperty("d4_pin")]
        public string D4Pin { get; set; }

        [JsonProperty("lcd_type")]
        public string LcdType { get; set; }

        [JsonProperty("d5_pin")]
        public string D5Pin { get; set; }

        [JsonProperty("e_pin")]
        public string EPin { get; set; }

        [JsonProperty("back_pin")]
        public string BackPin { get; set; }

        [JsonProperty("rs_pin")]
        public string RsPin { get; set; }

        [JsonProperty("analog_range_down_pin")]
        public string AnalogRangeDownPin { get; set; }

        [JsonProperty("analog_range_up_pin")]
        public string AnalogRangeUpPin { get; set; }

        [JsonProperty("up_pin")]
        public string UpPin { get; set; }

        [JsonProperty("d7_pin")]
        public string D7Pin { get; set; }

        [JsonProperty("analog_range_click_pin")]
        public string AnalogRangeClickPin { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
