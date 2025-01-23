using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
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

        [JsonProperty("print_start_time")]
        public partial double? PrintStartTime { get; set; }

        [ObservableProperty]

        [JsonIgnore]
        public partial double EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonProperty("size")]
        public partial long FileSize { get; set; }

        [ObservableProperty]

        [JsonProperty("modified")]
        public partial double Modified { get; set; }

        [ObservableProperty]

        [JsonProperty("slicer")]
        public partial string Slicer { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("slicer_version")]
        public partial string SlicerVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("layer_height")]
        public partial double LayerHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("first_layer_height")]
        public partial double FirstLayerHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("object_height")]
        public partial double ObjectHeight { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("filament_total")]
        public partial double FilamentTotal { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("filament_weight_total")]
        public partial double FilamentWeightTotal { get; set; } = 0;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(EstimatedPrintTime))]
        [JsonProperty("estimated_time")]
        public partial double EstimatedTime { get; set; } = 0;

        partial void OnEstimatedTimeChanged(double value)
        {
            EstimatedPrintTime = value;
        }

        [ObservableProperty]

        [JsonProperty("first_layer_extr_temp")]
        public partial double FirstLayerExtrTemp { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("first_layer_bed_temp")]
        public partial double FirstLayerBedTemp { get; set; } = 0;

        [ObservableProperty]

        [JsonProperty("gcode_start_byte")]
        public partial long GcodeStartByte { get; set; }

        [ObservableProperty]

        [JsonProperty("gcode_end_byte")]
        public partial long GcodeEndByte { get; set; }

        [ObservableProperty]

        [JsonProperty("job_id")]
        public partial string JobId { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("filename")]
        public partial string FileName { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("thumbnails")]
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
