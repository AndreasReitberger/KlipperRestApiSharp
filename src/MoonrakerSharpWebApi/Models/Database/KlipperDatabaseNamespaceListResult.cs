using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseNamespaceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("namespaces")]
        public partial List<string> Namespaces { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
