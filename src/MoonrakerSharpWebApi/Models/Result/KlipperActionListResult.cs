using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperActionListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("objects")]
        List<string> objects = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
