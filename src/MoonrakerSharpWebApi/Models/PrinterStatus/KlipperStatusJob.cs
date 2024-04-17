using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusJob : ObservableObject, IPrint3dJobStatus
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(EndTimeGeneralized))]
        [property: JsonProperty("end_time")]
        double? endTime;
        partial void OnEndTimeChanged(double? value)
        {
            if (value is not null)
                EndTimeGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        DateTime? endTimeGeneralized;

        [ObservableProperty]
        [JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName = string.Empty;

        [ObservableProperty]
        [JsonProperty("metadata")]
        IGcodeMeta? meta;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        [property: JsonProperty("print_duration")]
        double? printDuration;
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        TimeSpan? printDurationGeneralized;
        /*
        [ObservableProperty]
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperJobStates status;
        partial void OnStatusChanged(KlipperJobStates value)
        {
            State = (int)value;
        }
        */

        [ObservableProperty]
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        Print3dJobState state;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(StartTimeGeneralized))]
        [property: JsonProperty("start_time")]
        double? startTime;
        partial void OnStartTimeChanged(double? value)
        {
            if (value is not null)
                StartTimeGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        DateTime? startTimeGeneralized;

        [ObservableProperty]
        [property: JsonIgnore]
        double? done;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(TotalPrintDurationGeneralized))]
        [property: JsonProperty("total_duration")]
        double? totalPrintDuration;
        partial void OnTotalPrintDurationChanged(double? value)
        {
            if (value is not null)
                TotalPrintDurationGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        TimeSpan? totalPrintDurationGeneralized;

        [ObservableProperty]
        [JsonProperty("job_id")]
        string jobId = string.Empty;

        [ObservableProperty]
        [JsonProperty("exists")]
        bool fileExists;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            // Ordinarily, we release unmanaged resources here;
            // but all are wrapped by safe handles.

            // Release disposable objects.
            if (disposing)
            {
                // Nothing to do here
            }
        }
        #endregion

        #region Clone

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
