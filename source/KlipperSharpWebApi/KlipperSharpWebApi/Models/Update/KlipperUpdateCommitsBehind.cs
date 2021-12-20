using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateCommitsBehind
    {
        #region Properties
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("tag")]
        public object Tag { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
