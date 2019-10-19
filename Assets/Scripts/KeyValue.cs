using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents Key-value pairs.
/// This is a hack to get unity to show Dictionaries in the Inspector.
/// </summary>
[System.Serializable]
public class KeyValue<K,V> {
    public K key;
    public V value;

    /// <summary>
    /// Parses a list of KeyValue pairs into an actually useful dictionary
    /// </summary>
    /// <returns></returns>
    public static Dictionary<K,V> Parse(KeyValue<K,V>[] pairs)
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        foreach (KeyValue<K,V> pair in pairs)
        {
            dict.Add(pair.key, pair.value);
        }

        return dict;
    }
}

[System.Serializable]
public class StringInt : KeyValue<string, int>
{ 
}
