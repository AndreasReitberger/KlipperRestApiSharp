using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcodeCommandInfo : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        public partial string Command { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool Sent { get; set; } = false;



        [ObservableProperty]
        public partial bool Succeeded { get; set; } = false;

        [ObservableProperty]
        public partial DateTime TimeStamp { get; set; } = DateTime.Now;

        #endregion

        #region Overrides

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
