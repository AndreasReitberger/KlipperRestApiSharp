using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("count")]
        public partial long Count { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("jobs")]
        public partial List<KlipperJobItem> Jobs { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
