using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperJobQueueItem
    {
        #region Properties
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("job_id")]
        public string JobId { get; set; }

        [JsonProperty("time_added")]
        public double TimeAdded { get; set; }

        [JsonProperty("time_in_queue")]
        public double TimeInQueue { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
