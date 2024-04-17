using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Extensions
{
    public static class DictionaryExtensions
    {
        #region Extensions
        public static ConcurrentDictionary<T1, T2> ToConcurrent<T1, T2>(this Dictionary<T1, T2> source) where T1 : notnull
        {
            ConcurrentDictionary<T1, T2> target = new();
            if (source == null) return target;
            foreach (KeyValuePair<T1, T2> keypair in source)
            {
                target.AddOrUpdate(keypair.Key, keypair.Value, (k, v) => keypair.Value);
            }
            return target;
        }
        #endregion
    }
}
