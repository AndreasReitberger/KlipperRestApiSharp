using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("users")]
        List<KlipperUser> users = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
