using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsGeneral
    {
        #region Properties
        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("instanceName")]
        public string InstanceName { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
