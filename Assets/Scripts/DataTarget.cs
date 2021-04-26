using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
    public class DataTarget : MonoBehaviour
    {
        [SerializeField] string targetName;   
        [SerializeField] WayangEvents events;

        void Update()
        {
            StateManager sm = TrackerManager.Instance.GetStateManager();
            IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();

            foreach (TrackableBehaviour tb in tbs)
            {
                targetName = tb.TrackableName;
                //Wayang wy = events.WayangDictionary[targetName];
                events.UpdateWayang?.Invoke(events.WayangDictionary[targetName]);
            }
        }
    }
}
