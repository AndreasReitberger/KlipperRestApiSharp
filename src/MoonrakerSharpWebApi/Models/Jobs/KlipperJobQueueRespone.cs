using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("result")]
        public partial KlipperJobQueueResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
