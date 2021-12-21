using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodeMetaResult
    {
        #region Properties
        [JsonProperty("print_start_time")]
        public double? PrintStartTime { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("modified")]
        public double Modified { get; set; }

        [JsonProperty("slicer")]
        public string Slicer { get; set; }

        [JsonProperty("slicer_version")]
        public string SlicerVersion { get; set; }

        [JsonProperty("layer_height")]
        public double LayerHeight { get; set; }

        [JsonProperty("first_layer_height")]
        public double FirstLayerHeight { get; set; }

        [JsonProperty("object_height")]
        public double ObjectHeight { get; set; }

        [JsonProperty("filament_total")]
        public double FilamentTotal { get; set; }

        [JsonProperty("estimated_time")]
        public double EstimatedTime { get; set; }

        [JsonProperty("first_layer_extr_temp")]
        public double FirstLayerExtrTemp { get; set; }

        [JsonProperty("first_layer_bed_temp")]
        public double FirstLayerBedTemp { get; set; }

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
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
