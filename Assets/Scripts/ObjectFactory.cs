using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour {

    private static ObjectFactory instance;
    public static ObjectFactory Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ObjectFactory>();
            return instance;
        }
    }

    // hack using structs to populate dictionary in inspector
    // this dictionary is so we can map a name in our pattern data e.g. "Fly" to its corresponding prefab
    public StringToPrefabMapping[] Prefabs;
    Dictionary<string, Transform> _prefabs;

    void Start()
    {
        PopulateDictionaries();
    }

    public Transform Instantiate(string name)
    {
        return Instantiate(_prefabs[name]);        
    }

    private void PopulateDictionaries()
    {
        _prefabs = new Dictionary<string, Transform>();
        for (int i = 0; i < Prefabs.Length; i++)
        {
            _prefabs[Prefabs[i].ObjectName] = Prefabs[i].Prefab;
        }
    }
}
