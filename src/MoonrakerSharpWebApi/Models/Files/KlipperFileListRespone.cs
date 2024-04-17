using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileListRespone
    {
        #region Properties
        [JsonProperty("result")]
        public List<KlipperFile> Result { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
