using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueItem : ObservableObject, IPrint3dJob
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("job_id")]
        string jobId;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(TimeAddedGeneralized))]
        [property: JsonProperty("time_added")]
        double? timeAdded;
        partial void OnTimeAddedChanged(double? value)
        {
            if(value is not null)
                TimeAddedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        DateTime? timeAddedGeneralized;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(TimeInQueueGeneralized))]
        [property: JsonProperty("time_in_queue")]
        double? timeInQueue;
        partial void OnTimeInQueueChanged(double? value)
        {
            if (value is not null)
                TimeInQueueGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        DateTime? timeInQueueGeneralized;
        #endregion

        #region Methods
        public Task<bool> StartJobAsync(IPrint3dServerClient client, string command, object? data) => client.StartJobAsync(this, command, data);

        public Task<bool> PauseJobAsync(IPrint3dServerClient client, string command, object? data) => client.PauseJobAsync(command, data);

        public Task<bool> StopJobAsync(IPrint3dServerClient client, string command, object? data) => client.StopJobAsync(command, data);

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
