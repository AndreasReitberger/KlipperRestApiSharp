using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job")]
        OctoprintApiJobResult? job;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("progress")]
        OctoprintApiJobInfoProgress? progress;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        string state = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
