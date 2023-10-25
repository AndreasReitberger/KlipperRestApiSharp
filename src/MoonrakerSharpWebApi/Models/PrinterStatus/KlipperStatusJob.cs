using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
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
        [property: JsonProperty("end_time")]
        double? endTime;
        partial void OnEndTimeChanged(double? value)
        {
            
        }

        [ObservableProperty]
        [JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("metadata")]
        IGcodeMeta meta;

        [ObservableProperty]
        [JsonProperty("print_duration")]
        double? printDuration;

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
        [property: JsonProperty("start_time")]
        double? startTime;
        partial void OnStartTimeChanged(double? value)
        {
            
        }

        [ObservableProperty]
        [property: JsonIgnore]
        double? done;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(Done))]
        [property: JsonProperty("total_duration")]
        double? totalPrintDuration;
        partial void OnTotalPrintDurationChanged(double? value)
        {

        }

        [ObservableProperty]
        [JsonProperty("job_id")]
        string jobId;

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
