using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeHelpRespone
    {
        #region Properties
        [JsonProperty("result")]
        public Dictionary<string, string> Result { get; set; }
        //public KlipperGcodeHelpResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
