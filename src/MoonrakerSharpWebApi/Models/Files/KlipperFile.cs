using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using AndreasReitberger.API.Print3dServer.Core.Enums;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFile : ObservableObject, IGcode
    {
        #region Properties

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("path")]
        string filePath;

        [ObservableProperty]
        [JsonProperty("modified")]
        double modified;

        [ObservableProperty]
        [JsonProperty("size")]
        long size;

        [ObservableProperty]
        [JsonProperty("permissions")]
        string permissions;

        #region JsonIgnore
        [ObservableProperty]
        [JsonIgnore]
        IGcodeMeta meta;

        [ObservableProperty]
        [JsonIgnore]
        byte[] thumbnail = Array.Empty<byte>();

        [ObservableProperty]
        [JsonIgnore]
        byte[] image = Array.Empty<byte>();

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
