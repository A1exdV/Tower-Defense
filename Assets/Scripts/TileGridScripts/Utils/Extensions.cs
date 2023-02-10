using System;
using System.Collections.Generic;
using System.Linq;

namespace TileGridScripts.Utils
{
    public static class Extensions
    {
        public static void ForEachIndexed<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            foreach (var (item, index) in source.Select((item, index) => (item, index))) action(item, index);
        }
    }
}