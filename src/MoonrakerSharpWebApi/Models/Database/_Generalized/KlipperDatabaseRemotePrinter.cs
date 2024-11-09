using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseRemotePrinter : ObservableObject, IPrinter3d
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        //[property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        string hostname = string.Empty;

        [ObservableProperty, JsonIgnore]
        long port;

        [ObservableProperty, JsonIgnore]
        long webPort;

        [ObservableProperty, JsonIgnore]
        object? settings;

        #region Interface

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string activeJobId = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isActive;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isOnline = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long? lineSent;

        [ObservableProperty, JsonIgnore]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long? layers;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        int? analysed;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? done;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string activeJobName = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? extruder1Temperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? extruder2Temperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? extruder3Temperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? extruder4Temperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? extruder5Temperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? heatedBedTemperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? heatedChamberTemperature = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double? printProgress = 0;
        partial void OnPrintProgressChanged(double? value)
        {
            if (value > 0)
                RemainingPrintDuration = value * PrintDuration;
        }

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(RemainingPrintDurationGeneralized))]
        [property: JsonIgnore]
        double? remainingPrintDuration = 0;
        partial void OnRemainingPrintDurationChanged(double? value)
        {
            if (value is not null)
                RemainingPrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleSeconds(value);
        }

        [ObservableProperty]
        TimeSpan? remainingPrintDurationGeneralized;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isPrinting = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isPaused = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        bool isSelected = false;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        byte[] currentPrintImage = [];

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintStartedGeneralized))]
        [property: JsonIgnore]
        double? printStarted = 0;
        partial void OnPrintStartedChanged(double? value)
        {
            if (value is not null)
                PrintStartedGeneralized = TimeBaseConvertHelper.FromUnixDate(value);
        }

        [ObservableProperty]
        DateTime? printStartedGeneralized;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintDurationGeneralized))]
        [property: JsonIgnore]
        double? printDuration = 0;
        partial void OnPrintDurationChanged(double? value)
        {
            if (value is not null)
                PrintDurationGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
            if (value > 0)
                RemainingPrintDuration = value * PrintProgress;
        }

        [ObservableProperty]
        TimeSpan? printDurationGeneralized;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(PrintDurationEstimatedGeneralized))]
        [property: JsonIgnore]
        double? printDurationEstimated = 0;
        partial void OnPrintDurationEstimatedChanged(double? value)
        {
            if (value is not null)
                PrintDurationEstimatedGeneralized = TimeBaseConvertHelper.FromDoubleHours(value);
        }

        [ObservableProperty]
        TimeSpan? printDurationEstimatedGeneralized;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        int? repeat;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string slug = string.Empty;

        [ObservableProperty, JsonIgnore]
        long? start;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        long? totalLines;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        string? activeJobState = string.Empty;

        [ObservableProperty, JsonIgnore]
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
