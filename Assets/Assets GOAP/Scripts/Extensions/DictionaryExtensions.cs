using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions {
    public static bool In<T>(this T x, HashSet<T> set) {
        return set.Contains(x);
    }

    public static bool In<K, V>(this KeyValuePair<K, V> x, Dictionary<K, V> dict) {
        return dict.Contains(x);
    }

    public static void UpdateWith<K, V>(this Dictionary<K, V> a, Dictionary<K, V> b) {
        foreach (var kvp in b) {
            a[kvp.Key] = kvp.Value;
        }
    }
}