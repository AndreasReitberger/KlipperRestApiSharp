using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingsDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("down_pin")]
        string downPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("click_pin")]
        string clickPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("d6_pin")]
        string d6Pin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("d4_pin")]
        string d4Pin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("lcd_type")]
        string lcdType = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("hd44780_protocol_init")]
        bool hd44780ProtocolInit;

        [ObservableProperty]
        [property: JsonProperty("analog_range_up_pin")]
        List<long> analogRangeUpPin = [];

        [ObservableProperty]
        [property: JsonProperty("analog_range_click_pin")]
        List<long> analogRangeClickPin = [];

        [ObservableProperty]
        [property: JsonProperty("rs_pin")]
        string rsPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("menu_root")]
        string menuRoot = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("menu_reverse_navigation")]
        bool menuReverseNavigation;

        [ObservableProperty]
        [property: JsonProperty("analog_range_back_pin")]
        List<long> analogRangeBackPin = [];

        [ObservableProperty]
        [property: JsonProperty("d5_pin")]
        string d5Pin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("encoder_steps_per_detent")]
        long encoderStepsPerDetent;

        [ObservableProperty]
        [property: JsonProperty("encoder_fast_rate")]
        double encoderFastRate;

        [ObservableProperty]
        [property: JsonProperty("up_pin")]
        string upPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("d7_pin")]
        string d7Pin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("analog_pullup_resistor")]
        long analogPullupResistor;

        [ObservableProperty]
        [property: JsonProperty("menu_timeout")]
        long menuTimeout;

        [ObservableProperty]
        [property: JsonProperty("e_pin")]
        string ePin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("back_pin")]
        string backPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("display_group")]
        string displayGroup = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("analog_range_down_pin")]
        List<long> analogRangeDownPin = [];

        [ObservableProperty]
        [property: JsonProperty("line_length")]
        long lineLength;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
