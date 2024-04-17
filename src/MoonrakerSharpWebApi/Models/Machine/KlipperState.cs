using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperState : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("active_state")]
        string activeState = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("sub_state")]
        string subState = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
