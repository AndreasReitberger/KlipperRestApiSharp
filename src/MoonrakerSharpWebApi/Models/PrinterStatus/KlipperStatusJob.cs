using AndreasReitberger.API.Moonraker.Enum;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusJob : ObservableObject, IPrint3dJob
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
        long? filamentUsed;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("metadata")]
        KlipperGcodeMetaResult metadata;

        [ObservableProperty]
        [JsonProperty("print_duration")]
        double? printDuration;

        [ObservableProperty]
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        KlipperJobStates status;

        [ObservableProperty]
        [JsonProperty("start_time")]
        double? startTime;

        [ObservableProperty]
        [JsonProperty("total_duration")]
        double? totalDuration;

        [ObservableProperty]
        [JsonProperty("job_id")]
        string jobId;

        [ObservableProperty]
        [JsonProperty("exists")]
        bool exists;
        #endregion

        #region Interface, unused

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? timeAdded = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? timeInQueue = 0;
        #endregion

        #region Methods
        public Task<bool> StartJobAsync(IPrint3dServerClient client, string command, object? data) => client?.StartJobAsync(this, command, data);

        public Task<bool> PauseJobAsync(IPrint3dServerClient client, string command, object? data) => client?.PauseJobAsync(command, data);

        public Task<bool> StopJobAsync(IPrint3dServerClient client, string command, object? data) => client?.StopJobAsync(command, data);

        public Task<bool> RemoveFromQueueAsync(IPrint3dServerClient client, string command, object? data) => client.RemoveJobAsync(this, command, data);

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
