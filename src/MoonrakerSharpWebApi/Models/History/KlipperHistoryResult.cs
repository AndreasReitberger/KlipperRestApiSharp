using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("count")]
        public partial long Count { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("jobs")]
        public partial List<KlipperJobItem> Jobs { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
