using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerConfigResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("config")]
        KlipperServerConfig? config;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
