using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Materi Events", menuName = "Wayang/New MateriEvents")]
public class MateriEvents : ScriptableObject
{
    public delegate void WayangUpdatedCallback();
    public WayangUpdatedCallback WayangUpdated = null;

    public Wayang TargetWayang = null;
    public string DetectedTarget = string.Empty;
}
