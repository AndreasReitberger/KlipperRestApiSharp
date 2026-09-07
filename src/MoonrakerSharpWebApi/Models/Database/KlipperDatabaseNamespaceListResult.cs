using System.Collections.Generic;

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
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseNamespaceListResult);
        #endregion
    }
}
