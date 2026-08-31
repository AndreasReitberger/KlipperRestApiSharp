using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("users")]
        public partial List<KlipperUser> Users { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
