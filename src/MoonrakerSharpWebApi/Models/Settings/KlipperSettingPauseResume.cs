using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingPauseResume : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("recover_velocity")]
        public partial long RecoverVelocity { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
