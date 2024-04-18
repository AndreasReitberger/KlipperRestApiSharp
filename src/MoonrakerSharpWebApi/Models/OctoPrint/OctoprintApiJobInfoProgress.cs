using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobInfoProgress : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("completion", NullValueHandling = NullValueHandling.Ignore)]
        double completion;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filepos", NullValueHandling = NullValueHandling.Ignore)]
        long filepos;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printTime", NullValueHandling = NullValueHandling.Ignore)]
        long printTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printTimeLeft", NullValueHandling = NullValueHandling.Ignore)]
        long printTimeLeft;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
