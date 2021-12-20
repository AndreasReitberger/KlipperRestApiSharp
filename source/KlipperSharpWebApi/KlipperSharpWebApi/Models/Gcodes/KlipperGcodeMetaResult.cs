using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodeMetaResult
    {
        #region Properties
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("modified")]
        public double Modified { get; set; }

        [JsonProperty("slicer")]
        public string Slicer { get; set; }

        [JsonProperty("first_layer_bed_temp")]
        public long FirstLayerBedTemp { get; set; }

        [JsonProperty("gcode_start_byte")]
        public long GcodeStartByte { get; set; }

        [JsonProperty("gcode_end_byte")]
        public long GcodeEndByte { get; set; }

        [JsonProperty("print_start_time")]
        public object PrintStartTime { get; set; }

        [JsonProperty("job_id")]
        public object JobId { get; set; }

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
