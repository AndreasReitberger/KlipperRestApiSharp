using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusQueryEndstops : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("last_query")]
        public partial KlipperEndstopQueryResult? LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
