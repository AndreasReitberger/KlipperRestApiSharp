using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class OctoprintApiPrinterProfilesResult
    {
        #region Properties
        [JsonProperty("profiles")]
        public Dictionary<string, OctoprintApiPrinter> Profiles { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
