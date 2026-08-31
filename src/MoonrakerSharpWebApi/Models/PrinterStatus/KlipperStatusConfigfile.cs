using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusConfigfile : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("warnings")]
        public partial List<object> Warnings { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("config")]
        public partial KlipperConfig? Config { get; set; }

        [ObservableProperty]

        [JsonPropertyName("settings")]
        public partial KlipperSettings? Settings { get; set; }

        [ObservableProperty]

        [JsonPropertyName("save_config_pending")]
        public partial bool SaveConfigPending { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
