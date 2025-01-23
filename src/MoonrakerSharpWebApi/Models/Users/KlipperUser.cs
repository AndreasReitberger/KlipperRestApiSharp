using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUser : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("username")]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("created_on")]
        public partial double CreatedOn { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
