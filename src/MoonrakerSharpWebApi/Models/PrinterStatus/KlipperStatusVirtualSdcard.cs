using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusVirtualSdcard : ObservableObject
    {
        #region Properties
        [JsonIgnore]
        public int PercentageProgress => GetPercentageProgress();

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PercentageProgress))]
        [property: JsonProperty("progress")]
        double? progress;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("file_position")]
        long? filePosition;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("is_active")]
        bool isActive;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("file_path")]
        string filePath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("file_size")]
        long? fileSize;
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
