using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusQueryEndstops : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("last_query")]
        KlipperEndstopQueryResult? lastQuery;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
