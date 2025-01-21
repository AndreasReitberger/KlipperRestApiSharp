using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobInfoProgress : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("completion", NullValueHandling = NullValueHandling.Ignore)]
        public partial double Completion { get; set; }

        [ObservableProperty]
        
        [JsonProperty("filepos", NullValueHandling = NullValueHandling.Ignore)]
        public partial long Filepos { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printTime", NullValueHandling = NullValueHandling.Ignore)]
        public partial long PrintTime { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printTimeLeft", NullValueHandling = NullValueHandling.Ignore)]
        public partial long PrintTimeLeft { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
