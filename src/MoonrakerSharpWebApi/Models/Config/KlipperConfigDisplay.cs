using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigDisplay : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("down_pin")]
        string downPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_back_pin")]
        string analogRangeBackPin = string.Empty;

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
        [property: JsonProperty("d5_pin")]
        string d5Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("e_pin")]
        string ePin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("back_pin")]
        string backPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rs_pin")]
        string rsPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_down_pin")]
        string analogRangeDownPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_up_pin")]
        string analogRangeUpPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("up_pin")]
        string upPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("d7_pin")]
        string d7Pin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("analog_range_click_pin")]
        string analogRangeClickPin = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
