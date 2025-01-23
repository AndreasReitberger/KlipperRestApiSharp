using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistorySingleJobResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("job")]
        public partial KlipperJobItem? Job { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
