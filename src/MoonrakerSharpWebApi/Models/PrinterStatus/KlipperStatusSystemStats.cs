using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusSystemStats : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sysload")]
        double? sysload;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("memavail")]
        long? memavail;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cputime")]
        double? cputime;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
