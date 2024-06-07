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

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filename")]
        string fileName = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("metadata")]
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
        partial void OnPrintDurationGeneralizedChanged(TimeSpan? value)
        {
            Done = CalculateProgress(totalPrintDuration: TotalPrintDurationGeneralized, currentPrintDuration: value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        Print3dJobState? state;

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

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? done;
        partial void OnDoneChanged(double? value)
        {
            if (value is not null)
                DonePercentage = value / 100;
            else
                DonePercentage = 0;
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? donePercentage;

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
        partial void OnTotalPrintDurationGeneralizedChanged(TimeSpan? value)
        {
            Done = CalculateProgress(totalPrintDuration: value, currentPrintDuration: PrintDurationGeneralized);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job_id")]
        string jobId = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("exists")]
        bool fileExists;

        [ObservableProperty, JsonIgnore]
        double? remainingPrintTime;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion

        #region Methods

        double? CalculateProgress(DateTime? startTime, DateTime? endTime, TimeSpan? currentPrintDuration)
        {
            TimeSpan? overallDuration = endTime - startTime;
            if (overallDuration?.Ticks > 0 && currentPrintDuration?.Ticks > 0)
            {
                double? progressDone = currentPrintDuration?.Ticks / (overallDuration?.Ticks / 100d);
                return progressDone;
            }
            else return 0d;
        }

        double? CalculateProgress(TimeSpan? totalPrintDuration, TimeSpan? currentPrintDuration)
        {
            if (totalPrintDuration?.Ticks > 0 && currentPrintDuration?.Ticks > 0)
            {
                double? progressDone = currentPrintDuration?.Ticks / (totalPrintDuration?.Ticks / 100d);
                return progressDone;
            }
            else return 0d;
        }

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
