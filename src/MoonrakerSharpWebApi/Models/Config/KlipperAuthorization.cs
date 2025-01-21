using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAuthorization : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("login_timeout")]
        public partial long LoginTimeout { get; set; }

        [ObservableProperty]
        
        [JsonProperty("force_logins")]
        public partial bool ForceLogins { get; set; }

        [ObservableProperty]
        
        [JsonProperty("cors_domains")]
        public partial List<string> CorsDomains { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("trusted_clients")]
        public partial List<string> TrustedClients { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
