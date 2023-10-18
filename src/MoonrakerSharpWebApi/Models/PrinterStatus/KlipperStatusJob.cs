using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        [JsonProperty("end_time")]
        double? endTime;

        [ObservableProperty]
        [JsonProperty("filament_used")]
        double? filamentUsed;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("metadata")]
        //[JsonConverter(typeof(StringGcodeMetaConverter), true)]
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

        [ObservableProperty]
        [JsonProperty("start_time")]
        double? startTime;

        [ObservableProperty]
        [JsonProperty("total_duration")]
        double? totalPrintDuration;

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
