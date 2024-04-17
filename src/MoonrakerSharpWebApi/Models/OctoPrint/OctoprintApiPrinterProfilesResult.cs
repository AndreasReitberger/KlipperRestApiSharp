using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterProfilesResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("profiles")]
        Dictionary<string, OctoprintApiPrinter> profiles = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
