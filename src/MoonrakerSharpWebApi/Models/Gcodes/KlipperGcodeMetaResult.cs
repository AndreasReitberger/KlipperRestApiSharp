using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMetaResult : ObservableObject, IGcodeMeta
    {
        #region Properties
        [ObservableProperty]

        [JsonIgnore]
        public partial Guid Id { get; set; }

        [ObservableProperty]

        [JsonPropertyName("print_start_time")]
        public partial double? PrintStartTime { get; set; }

        [ObservableProperty]

        [JsonIgnore]
        public partial double EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("size")]
        public partial long FileSize { get; set; }

        [ObservableProperty]

        [JsonPropertyName("modified")]
        public partial double Modified { get; set; }

        [ObservableProperty]

        [JsonPropertyName("slicer")]
        public partial string Slicer { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("slicer_version")]
        public partial string SlicerVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("layer_height")]
        public partial double LayerHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("first_layer_height")]
        public partial double FirstLayerHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("object_height")]
        public partial double ObjectHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("filament_total")]
        public partial double FilamentTotal { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("filament_weight_total")]
        public partial double FilamentWeightTotal { get; set; } = 0;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(EstimatedPrintTime))]
        [JsonPropertyName("estimated_time")]
        public partial double EstimatedTime { get; set; } = 0;

        partial void OnEstimatedTimeChanged(double value)
        {
            EstimatedPrintTime = value;
        }

        [ObservableProperty]

        [JsonPropertyName("first_layer_extr_temp")]
        public partial double FirstLayerExtrTemp { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("first_layer_bed_temp")]
        public partial double FirstLayerBedTemp { get; set; } = 0;

        [ObservableProperty]

        [JsonPropertyName("gcode_start_byte")]
        public partial long GcodeStartByte { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_end_byte")]
        public partial long GcodeEndByte { get; set; }

        [ObservableProperty]

        [JsonPropertyName("job_id")]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("filename")]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("thumbnails")]
        public partial List<IGcodeImage> GcodeImages { get; set; } = [];

        [JsonIgnore]
        public long Layers => GetLayersCount();
        #endregion

        #region Methods
        long GetLayersCount()
        {
            try
            {
                return LayerHeight <= 0 ? 0 : Convert.ToInt64((ObjectHeight - FirstLayerHeight) / LayerHeight) + 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMetaResult);
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
