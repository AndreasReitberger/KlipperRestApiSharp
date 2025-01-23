using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusSystemStats : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("sysload")]
        public partial double? Sysload { get; set; }

        [ObservableProperty]

        [JsonProperty("memavail")]
        public partial long? Memavail { get; set; }

        [ObservableProperty]

        [JsonProperty("cputime")]
        public partial double? Cputime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
