using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
    public class DataTarget : MonoBehaviour
    {
        [SerializeField] string targetName;   
        [SerializeField] WayangEvents events;
        public DefaultObserverEventHandler observerEventHandler;

		private void Awake()
		{
			if (observerEventHandler != null) observerEventHandler = GetComponent<DefaultObserverEventHandler>();
		}

		private void OnEnable()
		{
            observerEventHandler.OnTargetFound.AddListener(OnTargetFound);
            observerEventHandler.OnTargetLost.AddListener(OnTargetLost);
		}

		private void OnDisable()
		{
			observerEventHandler.OnTargetFound.RemoveListener(OnTargetFound);
			observerEventHandler.OnTargetLost.RemoveListener(OnTargetLost);
		}

		private void OnTargetFound()
		{
			events.UpdateWayang?.Invoke(events.WayangDictionary[targetName]);
		}

		private void OnTargetLost()
		{
			events.UpdateWayang?.Invoke(null);
		}

		/*void Update()
        {
            //StateManager sm = VuforiaBehaviour.Instance.World.Instance.GetStateManager();
            //IEnumerable<TrackableBehaviour> tbs = sm.GetActiveTrackableBehaviours();
            IEnumerable<ObserverBehaviour> tbs = VuforiaBehaviour.Instance.World.GetTrackedObserverBehaviours();

			foreach (ObserverBehaviour tb in tbs)
            {
                targetName = tb.TargetName;
                //Wayang wy = events.WayangDictionary[targetName];
                events.UpdateWayang?.Invoke(events.WayangDictionary[targetName]);
            }
        }*/

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (observerEventHandler is null) observerEventHandler = GetComponent<DefaultObserverEventHandler>();
		}
#endif
	}
}
