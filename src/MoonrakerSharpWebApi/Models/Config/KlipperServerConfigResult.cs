using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfigResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("config")]
        public partial KlipperServerConfig? Config { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
