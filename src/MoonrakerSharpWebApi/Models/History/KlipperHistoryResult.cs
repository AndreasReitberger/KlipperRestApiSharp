using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("count")]
        long count = 0;

        [ObservableProperty]
        [property: JsonProperty("jobs")]
        List<KlipperJobItem> jobs = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
