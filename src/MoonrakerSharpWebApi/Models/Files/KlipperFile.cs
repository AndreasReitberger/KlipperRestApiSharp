using AndreasReitberger.API.Print3dServer.Core.Enums;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFile : ObservableObject, IGcode
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        GcodeTimeBaseTarget timeBaseTarget = GcodeTimeBaseTarget.DoubleHoursUnix;

        [ObservableProperty]
        [JsonProperty("filename")]
        [property: JsonIgnore]
        string fileName;

        [ObservableProperty]
        [JsonProperty("path")]
        [property: JsonIgnore]
        string filePath;

        [ObservableProperty]
        [JsonProperty("modified")]
        [property: JsonIgnore]
        double modified;

        [ObservableProperty]
        [JsonProperty("size")]
        [property: JsonIgnore]
        long size;

        [ObservableProperty]
        [JsonProperty("permissions")]
        [property: JsonIgnore]
        string permissions;

        #region JsonIgnore
        [ObservableProperty]
        [JsonIgnore]
        IGcodeMeta meta;
        partial void OnMetaChanged(IGcodeMeta value)
        {
            if (value is not null)
            {
                PrintTime = value.EstimatedPrintTime;
            }
        }

        [ObservableProperty]
        [JsonIgnore]
        byte[] thumbnail = [];

        [ObservableProperty]
        [JsonIgnore]
        byte[] image = [];

        [ObservableProperty]
        [JsonIgnore]
        double filament;

        [ObservableProperty]
        [JsonIgnore]
        GcodeImageType imageType = GcodeImageType.Thumbnail;

        [ObservableProperty]
        [JsonIgnore]
        string printerName = string.Empty;

        [ObservableProperty]
        [JsonIgnore]
        string group;

        [ObservableProperty]
        [JsonIgnore]
        long identifier;

        [ObservableProperty]
        [JsonIgnore]
        double volume;

        [ObservableProperty]
        double printTime;
        partial void OnPrintTimeChanged(double value)
        {
            PrintTimeGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);          
        }

        [ObservableProperty]
        TimeSpan? printTimeGeneralized;
        #endregion

        #endregion

        #region Methods
        public Task MoveToAsync(IPrint3dServerClient client, string targetPath, bool copy = false)
        {
            throw new NotImplementedException();
        }

        public Task MoveToQueueAsync(IPrint3dServerClient client, bool printIfReady = false)
        {
            throw new NotImplementedException();
        }

        public Task PrintAsync(IPrint3dServerClient client)
        {
            throw new NotImplementedException();
        }
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

        public object Clone() => MemberwiseClone();

        #endregion
    }
}
