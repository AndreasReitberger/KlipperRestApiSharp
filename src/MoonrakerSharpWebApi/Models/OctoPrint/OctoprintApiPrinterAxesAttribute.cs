using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxesAttribute : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("inverted")]
        bool inverted;

        [ObservableProperty]
        [property: JsonProperty("speed")]
        long speed;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
