using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusVirtualSdcard : ObservableObject
    {
        #region Properties
        [JsonIgnore]
        public int PercentageProgress => GetPercentageProgress();

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(PercentageProgress))]
        [JsonProperty("progress")]
        public partial double? Progress { get; set; }

        [ObservableProperty]

        [JsonProperty("file_position")]
        public partial long? FilePosition { get; set; }

        [ObservableProperty]

        [JsonProperty("is_active")]
        public partial bool IsActive { get; set; }

        [ObservableProperty]

        [JsonProperty("file_path")]
        public partial string FilePath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("file_size")]
        public partial long? FileSize { get; set; }
        #endregion

        #region Methods
        int GetPercentageProgress()
        {
            try
            {
                if (Progress == null || Progress <= 0) return 0;
                int calc = Convert.ToInt32(Progress * 100);
                return calc;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
