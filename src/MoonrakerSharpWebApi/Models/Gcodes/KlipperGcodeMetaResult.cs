using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeMetaResult : ObservableObject, IGcodeMeta
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_start_time")]
        double? printStartTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonIgnore]
        double estimatedPrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("size")]
        long fileSize;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("modified")]
        double modified;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("slicer")]
        string slicer = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("slicer_version")]
        string slicerVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("layer_height")]
        double layerHeight = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("first_layer_height")]
        double firstLayerHeight = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("object_height")]
        double objectHeight = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_total")]
        double filamentTotal = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_weight_total")]
        double filamentWeightTotal = 0;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(EstimatedPrintTime))]
        [property: JsonProperty("estimated_time")]
        double estimatedTime = 0;
        partial void OnEstimatedTimeChanged(double value)
        {
            EstimatedPrintTime = value;
        }

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("first_layer_extr_temp")]
        double firstLayerExtrTemp = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("first_layer_bed_temp")]
        double firstLayerBedTemp = 0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_start_byte")]
        long gcodeStartByte;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_end_byte")]
        long gcodeEndByte;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job_id")]
        string jobId = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filename")]
        string fileName = string.Empty;    

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("thumbnails")]
        ObservableCollection<IGcodeImage> gcodeImages = [];

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
