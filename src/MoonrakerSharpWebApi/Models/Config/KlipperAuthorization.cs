using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperAuthorization : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("login_timeout")]
        long loginTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("force_logins")]
        bool forceLogins;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cors_domains")]
        List<string> corsDomains = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("trusted_clients")]
        List<string> trustedClients = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
