using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUser : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("username")]
        string username = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("created_on")]
        double createdOn;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
