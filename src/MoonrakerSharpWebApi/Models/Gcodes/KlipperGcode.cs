using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcode : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("message")]
        string message = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("time")]
        double time;

        [ObservableProperty]
        [property: JsonProperty("type")]
        string type = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
