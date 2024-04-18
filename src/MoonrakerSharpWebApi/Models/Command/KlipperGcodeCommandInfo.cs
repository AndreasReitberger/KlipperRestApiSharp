using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeCommandInfo : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        Guid id;

        [ObservableProperty]
        string command = string.Empty;

        [ObservableProperty]
        bool sent = false;
        [JsonIgnore]

        [ObservableProperty]
        bool succeeded = false;

        [ObservableProperty]
        DateTime timeStamp = DateTime.Now;

        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
