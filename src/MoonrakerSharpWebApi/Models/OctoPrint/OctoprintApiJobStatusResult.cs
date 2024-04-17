using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("job")]
        OctoprintApiJobResult? job;

        [ObservableProperty]
        [property: JsonProperty("progress")]
        OctoprintApiJobInfoProgress? progress;

        [ObservableProperty]
        [property: JsonProperty("state")]
        string state = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
