using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingsDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("down_pin")]
        string downPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("click_pin")]
        string clickPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("d6_pin")]
        string d6Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("d4_pin")]
        string d4Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("lcd_type")]
        string lcdType = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hd44780_protocol_init")]
        bool hd44780ProtocolInit;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_up_pin")]
        List<long> analogRangeUpPin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_click_pin")]
        List<long> analogRangeClickPin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rs_pin")]
        string rsPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("menu_root")]
        string menuRoot = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("menu_reverse_navigation")]
        bool menuReverseNavigation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_back_pin")]
        List<long> analogRangeBackPin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("d5_pin")]
        string d5Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("encoder_steps_per_detent")]
        long encoderStepsPerDetent;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("encoder_fast_rate")]
        double encoderFastRate;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("up_pin")]
        string upPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("d7_pin")]
        string d7Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_pullup_resistor")]
        long analogPullupResistor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("menu_timeout")]
        long menuTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("e_pin")]
        string ePin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("back_pin")]
        string backPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display_group")]
        string displayGroup = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_down_pin")]
        List<long> analogRangeDownPin = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("line_length")]
        long lineLength;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
