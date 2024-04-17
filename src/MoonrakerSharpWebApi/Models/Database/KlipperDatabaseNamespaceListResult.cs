using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseNamespaceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("namespaces")]
        List<string> namespaces = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
