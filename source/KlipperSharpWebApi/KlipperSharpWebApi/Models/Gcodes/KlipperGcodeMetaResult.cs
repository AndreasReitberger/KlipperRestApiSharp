using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodeMetaResult
    {
        #region Properties
        [JsonProperty("print_start_time")]
        public double? PrintStartTime { get; set; }

        [JsonProperty("size")]
        public long FileSize { get; set; }

        [JsonProperty("modified")]
        public double Modified { get; set; }

        [JsonProperty("slicer")]
        public string Slicer { get; set; }

        [JsonProperty("slicer_version")]
        public string SlicerVersion { get; set; }

        [JsonProperty("layer_height")]
        public double LayerHeight { get; set; } = 0;

        [JsonProperty("first_layer_height")]
        public double FirstLayerHeight { get; set; } = 0;

        [JsonProperty("object_height")]
        public double ObjectHeight { get; set; } = 0;

        [JsonProperty("filament_total")]
        public double FilamentTotal { get; set; } = 0;

        [JsonProperty("filament_weight_total")]
        public double FilamentWeightTotal { get; set; } = 0;

        [JsonProperty("estimated_time")]
        public double EstimatedTime { get; set; } = 0;

        [JsonProperty("first_layer_extr_temp")]
        public double FirstLayerExtrTemp { get; set; } = 0;

        [JsonProperty("first_layer_bed_temp")]
        public double FirstLayerBedTemp { get; set; } = 0;

        [JsonProperty("gcode_start_byte")]
        public long GcodeStartByte { get; set; }

        [JsonProperty("gcode_end_byte")]
        public long GcodeEndByte { get; set; }

        [JsonProperty("job_id")]
        public string JobId { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("thumbnails")]
        public List<KlipperGcodeThumbnail> Thumbnails { get; set; }

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
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
