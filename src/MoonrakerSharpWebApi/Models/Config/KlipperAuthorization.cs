using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAuthorization : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("login_timeout")]
        public partial long LoginTimeout { get; set; }

        [ObservableProperty]
        [JsonPropertyName("force_logins")]
        public partial bool ForceLogins { get; set; }

        [ObservableProperty]
        [JsonPropertyName("cors_domains")]
        public partial List<string> CorsDomains { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("trusted_clients")]
        public partial List<string> TrustedClients { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperAuthorization);
        #endregion
    }
}
