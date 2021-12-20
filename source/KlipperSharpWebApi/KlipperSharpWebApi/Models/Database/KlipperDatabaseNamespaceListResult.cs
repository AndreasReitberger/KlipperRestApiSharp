using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseNamespaceListResult
    {
        #region Properties
        [JsonProperty("namespaces")]
        public List<string> Namespaces { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
