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

        [ObservableProperty]
        [JsonProperty("print_start_time")]
        double? printStartTime;

        [ObservableProperty]
        [property: JsonIgnore]
        double estimatedPrintTime;

        [ObservableProperty]
        [JsonProperty("size")]
        long fileSize;

        [ObservableProperty]
        [JsonProperty("modified")]
        double modified;

        [ObservableProperty]
        [JsonProperty("slicer")]
        string slicer;

        [ObservableProperty]
        [JsonProperty("slicer_version")]
        string slicerVersion;

        [ObservableProperty]
        [JsonProperty("layer_height")]
        double layerHeight = 0;

        [ObservableProperty]
        [JsonProperty("first_layer_height")]
        double firstLayerHeight = 0;

        [ObservableProperty]
        [JsonProperty("object_height")]
        double objectHeight = 0;

        [ObservableProperty]
        [JsonProperty("filament_total")]
        double filamentTotal = 0;

        [ObservableProperty]
        [JsonProperty("filament_weight_total")]
        double filamentWeightTotal = 0;

        [ObservableProperty]
        [JsonProperty("estimated_time")]
        double estimatedTime = 0;

        [ObservableProperty]
        [JsonProperty("first_layer_extr_temp")]
        double firstLayerExtrTemp = 0;

        [ObservableProperty]
        [JsonProperty("first_layer_bed_temp")]
        double firstLayerBedTemp = 0;

        [ObservableProperty]
        [JsonProperty("gcode_start_byte")]
        long gcodeStartByte;

        [ObservableProperty]
        [JsonProperty("gcode_end_byte")]
        long gcodeEndByte;

        [ObservableProperty]
        [JsonProperty("job_id")]
        string jobId;

        [ObservableProperty]
        [JsonProperty("filename")]
        string fileName;

        [ObservableProperty]
        [JsonProperty("thumbnails")]
        ObservableCollection<IGcodeImage> gcodeImages = new();

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
