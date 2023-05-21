using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Extensions
{
    public static class DictionaryExtensions
    {
        #region Extensions
        public static ConcurrentDictionary<T1, T2> ToConcurrent<T1, T2>(this Dictionary<T1, T2> source)
        {
            ConcurrentDictionary<T1, T2> target = new();
            foreach (var keypair in source)
            {
                target.AddOrUpdate(keypair.Key, keypair.Value, (k, v) => keypair.Value);
            }
            return target;
        }
        #endregion
    }
}
