using AndreasReitberger.API.Moonraker.Enum;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AndreasReitberger.API.Moonraker
{
    public partial class MoonrakerClient
    {
        #region Static
        public static ObservableCollection<MoonrakerOperatingSystems> SupportedOperatingSystems = new(
            System.Enum.GetValues(typeof(MoonrakerOperatingSystems)).Cast<MoonrakerOperatingSystems>());

        public static ObservableCollection<string> SupportedOperatingSystemNames = new(
            SupportedOperatingSystems.Select(item => item.ToString()));

        public static void AddToConcurrentDictionary<T1, T2>(Dictionary<T1, T2> source, ConcurrentDictionary<T1, T2> target)
        {
            foreach(KeyValuePair<T1, T2> keypair  in source)
            {
                target.AddOrUpdate(keypair.Key, keypair.Value, (k, v) => keypair.Value);
            }
        }
        public static ConcurrentDictionary<T1, T2> ToConcurrent<T1, T2>(Dictionary<T1, T2> source)
        {
            ConcurrentDictionary<T1, T2> target = new();
            if (source == null) return target;
            foreach (KeyValuePair<T1, T2> keypair  in source)
            {
                target.AddOrUpdate(keypair.Key, keypair.Value, (k, v) => keypair.Value);
            }
            return target;
        }
        public static Dictionary<T1, T2> ToDictionary<T1, T2>(ConcurrentDictionary<T1, T2> source)
        {
            Dictionary<T1, T2> target = new();
            if (source == null) return target;
            foreach (KeyValuePair<T1, T2> keypair  in source)
            {
                target.TryAdd(keypair.Key, keypair.Value);
            }
            return target;
        }
        #endregion
    }
}
