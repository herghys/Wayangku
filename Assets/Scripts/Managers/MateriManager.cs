using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MateriManager : MonoBehaviour
{
    #region Variables
    [SerializeField] string targetName;
    [SerializeField] List<Wayang> wayang;

    [Header("References")]
    [SerializeField] WayangEvents events = null;
    #endregion

    #region Unity Defaults
    private void OnEnable()
    {
        events.UpdateWayang += UpdateWayangObject;
    }

    private void OnDisable()
    {
        events.UpdateWayang -= UpdateWayangObject;
    }
    #endregion

    #region Methods
    public void GetWayangName(string name)
    {
        events.UpdateWayang?.Invoke(events.WayangDictionary[name]);
        //UpdateWayangObject(events.WayangDictionary[name]);
    }

    public void CloseGroup()
    {
        events.UIUpdate?.Invoke(false);
    }

    private void UpdateWayangObject(Wayang wy)
    {
        events.UIUpdate?.Invoke(true);
    }
    #endregion
}
