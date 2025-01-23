using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingForceMove : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("enable_force_move")]
        public partial bool EnableForceMove { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
