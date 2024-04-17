using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAuthorization
    {
        #region Properties
        [JsonProperty("login_timeout")]
        public long LoginTimeout { get; set; }

        [JsonProperty("force_logins")]
        public bool ForceLogins { get; set; }

        [JsonProperty("cors_domains")]
        public List<string> CorsDomains { get; set; } = new();

        [JsonProperty("trusted_clients")]
        public List<string> TrustedClients { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
