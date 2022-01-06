using AndreasReitberger.Enum;
using System.Collections.ObjectModel;
using System.Linq;

namespace AndreasReitberger
{
    public partial class KlipperClient
    {
        #region Static
        public static ObservableCollection<MoonrakerOperatingSystems> SupportedOperatingSystems = new(
            System.Enum.GetValues(typeof(MoonrakerOperatingSystems)).Cast<MoonrakerOperatingSystems>());

        public static ObservableCollection<string> SupportedOperatingSystemNames = new(
            SupportedOperatingSystems.Select(item => item.ToString()));
        #endregion
    }
}
