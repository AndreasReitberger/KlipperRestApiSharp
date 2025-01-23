using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterProfilesResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("profiles")]
        public partial Dictionary<string, OctoprintApiPrinter> Profiles { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
