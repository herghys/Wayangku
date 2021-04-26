using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WayangEvents", menuName = "Wayang/New WayangEvents")]
public class WayangEvents : ScriptableObject
{
    public delegate void UpdateDescriptionCallback(Wayang wayang);
    public UpdateDescriptionCallback UpdateWayang = null;

    public delegate void MateriUICallback(bool state);
    public MateriUICallback UIUpdate = null;

    Dictionary<string, Wayang> wayangDictionary = new Dictionary<string, Wayang>();
    public Dictionary<string, Wayang> WayangDictionary 
    { 
        get { return wayangDictionary; } 
        set { wayangDictionary = value; }
    }
}
