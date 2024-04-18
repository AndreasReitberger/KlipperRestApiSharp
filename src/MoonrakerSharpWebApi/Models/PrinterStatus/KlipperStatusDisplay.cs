using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDisplay : ObservableObject
    {
        #region Properties
        [JsonIgnore]
        public int PercentageProgress => GetPercentageProgress();

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PercentageProgress))]
        [property: JsonProperty("progress")]
        double? progress;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("message")]
        string message = string.Empty;
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
