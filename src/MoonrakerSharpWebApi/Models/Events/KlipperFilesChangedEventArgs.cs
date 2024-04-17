using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use `GcodesChangedEventArgs` instead")]
    public class KlipperFilesChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperFile> NewFiles { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
