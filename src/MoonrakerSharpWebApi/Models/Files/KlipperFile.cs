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

        [ObservableProperty]

        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]

        [JsonIgnore]
        public partial GcodeTimeBaseTarget TimeBaseTarget { get; set; } = GcodeTimeBaseTarget.DoubleHoursUnix;

        [ObservableProperty]

        [JsonProperty("filename")]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("path")]
        public partial string FilePath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("modified")]
        public partial double? Modified { get; set; }

        partial void OnModifiedChanged(double? value)
        {
            Created = value;
        }

        [ObservableProperty]

        [JsonProperty("size")]
        public partial long Size { get; set; }

        [ObservableProperty]

        [JsonProperty("permissions")]
        public partial string Permissions { get; set; } = string.Empty;

        #region JsonIgnore

        [ObservableProperty]

        [JsonProperty("created")]
        [NotifyPropertyChangedFor(nameof(CreatedGeneralized))]
        public partial double? Created { get; set; } = 0;

        partial void OnCreatedChanged(double? value)
        {
            if (value is not null)
                CreatedGeneralized = TimeBaseConvertHelper.FromUnixDoubleMiliseconds(value);
        }

        [ObservableProperty]

        public partial DateTime? CreatedGeneralized { get; set; }

        [ObservableProperty]

        public partial IGcodeMeta? Meta { get; set; }

        partial void OnMetaChanged(IGcodeMeta? value)
        {
            if (value is not null)
            {
                PrintTime = value.EstimatedPrintTime;
            }
        }

        [ObservableProperty]

        public partial byte[]? Thumbnail { get; set; } = [];

        [ObservableProperty]

        public partial byte[]? Image { get; set; } = [];

        [ObservableProperty]

        public partial double Filament { get; set; }

        [ObservableProperty]

        public partial GcodeImageType ImageType { get; set; } = GcodeImageType.Thumbnail;

        [ObservableProperty]

        public partial string PrinterName { get; set; } = string.Empty;

        [ObservableProperty]

        public partial string Group { get; set; } = string.Empty;

        [ObservableProperty]

        public partial long Identifier { get; set; }

        [ObservableProperty]

        public partial double Volume { get; set; }

        [ObservableProperty]

        public partial double PrintTime { get; set; }

        partial void OnPrintTimeChanged(double value)
        {
            PrintTimeGeneralized = TimeBaseConvertHelper.FromUnixDoubleHours(value);
        }

        [ObservableProperty]

        public partial TimeSpan? PrintTimeGeneralized { get; set; }
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
