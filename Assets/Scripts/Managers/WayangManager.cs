using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayangManager : MonoBehaviour
{
    [SerializeField] WayangEvents events;
    void Start()
    {
        events.WayangDictionary.Clear();
        LoadWayang();
    }

    void LoadWayang()
    {
        var objs = Resources.LoadAll("Wayang", typeof(Wayang));
        foreach (Wayang item in objs)
        {
            events.WayangDictionary.Add(item.name, item);
        }
    }
}