using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseRemotePrinter : ObservableObject, IPrinter3d
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty]
        string hostname = string.Empty;

        [ObservableProperty]
        long port;

        [ObservableProperty]
        long webPort;

        [ObservableProperty]
        object? settings;

        #region Interface

        [ObservableProperty]
        [property: JsonIgnore]
        string activeJobId = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore]
        bool isActive;

        [ObservableProperty]
        [property: JsonIgnore]
        bool isOnline = false;

        [ObservableProperty]
        [property: JsonIgnore]
        long? lineSent;

        [ObservableProperty]
        [property: JsonIgnore]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore]
        long? layers;

        [ObservableProperty]
        [property: JsonIgnore]
        int? analysed;

        [ObservableProperty]
        [property: JsonIgnore]
        double? done;

        [ObservableProperty]
        [property: JsonIgnore]
        string activeJobName = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder1Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder2Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder3Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder4Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? extruder5Temperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? heatedBedTemperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? heatedChamberTemperature = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? printProgress = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        double? remainingPrintDuration = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        bool isPrinting = false;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        bool isPaused = false;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        bool isSelected = false;

        [ObservableProperty]
        [property: JsonIgnore]
        [JsonIgnore]
        byte[] currentPrintImage = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? printStarted = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? printDuration = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        double? printDurationEstimated = 0;

        [ObservableProperty]
        [property: JsonIgnore]
        int? repeat;

        [ObservableProperty]
        [property: JsonIgnore]
        string slug = string.Empty;

        [ObservableProperty]
        long? start;

        [ObservableProperty]
        [property: JsonIgnore]
        long? totalLines;

        [ObservableProperty]
        [property: JsonIgnore]
        string? activeJobState = string.Empty;

        [ObservableProperty]
        [property: JsonIgnore]
        long? pauseState;

        #endregion

        #endregion

        #region Methods

        public Task<bool> HomeAsync(IPrint3dServerClient client, bool x, bool y, bool z) => client.HomeAsync(x, y, z);

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public override bool Equals(object? obj)
        {
            if (obj is not KlipperDatabaseRemotePrinter item)
                return false;
            return Equals(item);
        }

        public override int GetHashCode() => Slug.GetHashCode();

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

        public object Clone() => MemberwiseClone();

        #endregion
    }
}
