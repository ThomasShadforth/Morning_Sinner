using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionUtility
{
    //K is the key
    //V is the value
    public static void AddItem<K, V>(this SerializableDictionary<K, List<V>> serializableDictionary, K key, V value)
    {
        if (serializableDictionary.ContainsKey(key))
        {
            serializableDictionary[key].Add(value);

            return;
        }

        //Collection initialising - allows lists to be initialised with specific values within it
        serializableDictionary.Add(key, new List<V>() { value });
    }
}
