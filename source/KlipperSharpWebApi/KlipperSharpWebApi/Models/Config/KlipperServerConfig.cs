using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperServerConfig
    {
        #region Properties
        [JsonProperty("server")]
        public KlipperServer Server { get; set; }

        [JsonProperty("authorization")]
        public KlipperAuthorization Authorization { get; set; }

        [JsonProperty("octoprint_compat")]
        public object OctoprintCompat { get; set; }

        [JsonProperty("history")]
        public object History { get; set; }

        [JsonProperty("update_manager")]
        public KlipperUpdateManager UpdateManager { get; set; }

        [JsonProperty("update_manager moonraker")]
        public KlipperUpdateManagerKlipperClass UpdateManagerMoonraker { get; set; }

        [JsonProperty("update_manager klipper")]
        public KlipperUpdateManagerKlipperClass UpdateManagerKlipper { get; set; }

        [JsonProperty("update_manager mainsail")]
        public KlipperUpdateManagerMainsail UpdateManagerMainsail { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
