using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusSystemStats : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("sysload")]
        double? sysload;

        [ObservableProperty]
        [property: JsonProperty("memavail")]
        long? memavail;

        [ObservableProperty]
        [property: JsonProperty("cputime")]
        double? cputime;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
