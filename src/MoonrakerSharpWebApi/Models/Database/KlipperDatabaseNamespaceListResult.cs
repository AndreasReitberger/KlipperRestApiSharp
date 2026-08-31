using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseNamespaceListResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("namespaces")]
        public partial List<string> Namespaces { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
