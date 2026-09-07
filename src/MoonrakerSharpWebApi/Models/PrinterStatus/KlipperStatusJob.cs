using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusJob : ObservableObject, IPrint3dJobStatus
    {
        #region Properties
        [ObservableProperty]
        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(EndTimeGeneralized))]
        [JsonPropertyName("end_time")]
        public partial double? EndTime { get; set; }
        partial void OnEndTimeChanged(double? value)
        {
            if (value is not null)
                EndTimeGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        [JsonIgnore]
        public partial DateTime? EndTimeGeneralized { get; set; }

        [ObservableProperty]
        [JsonPropertyName("filament_used")]
        public partial double? FilamentUsed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("filename")]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("metadata")]
        public partial IGcodeMeta? Meta { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        [JsonPropertyName("print_duration")]
        public partial double? PrintDuration { get; set; }
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);
        }

        [ObservableProperty]
        [JsonIgnore]
        public partial TimeSpan? PrintDurationGeneralized { get; set; }
        partial void OnPrintDurationGeneralizedChanged(TimeSpan? value)
        {
            Done = CalculateProgress(totalPrintDuration: TotalPrintDurationGeneralized, currentPrintDuration: value);
        }

        [ObservableProperty]
        [JsonPropertyName("status")]
        //[field: JsonConverter(typeof(StringEnumConverter), true)]
        public partial Print3dJobState? State { get; set; }

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(StartTimeGeneralized))]
        [JsonPropertyName("start_time")]
        public partial double? StartTime { get; set; }
        partial void OnStartTimeChanged(double? value)
        {
            if (value is not null)
                StartTimeGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        [JsonIgnore]
        public partial DateTime? StartTimeGeneralized { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public partial double? Done { get; set; }
        partial void OnDoneChanged(double? value)
        {
            if (value is not null)
                DonePercentage = value / 100;
            else
                DonePercentage = 0;
        }

        [ObservableProperty]
        [JsonIgnore]
        public partial double? DonePercentage { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Done))]
        [NotifyPropertyChangedFor(nameof(TotalPrintDurationGeneralized))]
        [JsonPropertyName("total_duration")]
        public partial double? TotalPrintDuration { get; set; }
        partial void OnTotalPrintDurationChanged(double? value)
        {
            if (value is not null)
                TotalPrintDurationGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);
        }

        [ObservableProperty]
        [JsonIgnore]
        public partial TimeSpan? TotalPrintDurationGeneralized { get; set; }
        partial void OnTotalPrintDurationGeneralizedChanged(TimeSpan? value)
        {
            Done = CalculateProgress(totalPrintDuration: value, currentPrintDuration: PrintDurationGeneralized);
        }

        [ObservableProperty]
        [JsonPropertyName("job_id")]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("exists")]
        public partial bool FileExists { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RemainingPrintTimeGeneralized))]
        public partial double? RemainingPrintTime { get; set; }
        partial void OnRemainingPrintTimeChanged(double? value)
        {
            if (value is not null)
                RemainingPrintTimeGeneralized = TimeBaseConvertHelper.FromDoubleSeconds(value);
        }

        [ObservableProperty]
        public partial TimeSpan? RemainingPrintTimeGeneralized { get; set; }

        [ObservableProperty]
        public partial long? Repeat { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusJob);

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
