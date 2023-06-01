using UnityEngine;
using Vuforia;
using System.Collections;
public class ModelSwapper : MonoBehaviour
{
    public ImageTargetBehaviour theTrackable;
    private bool mSwapModel = false;
    // Use this for initialization
    void Start()
    {
        if (theTrackable == null)
        {
            Debug.Log("Warning: Trackable not set !!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (mSwapModel && theTrackable != null)
        {
            SwapModel();
            mSwapModel = false;
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 120, 40), "Swap Model"))
        {
            mSwapModel = true;
        }
    }
    private void SwapModel()
    {
        GameObject trackableGameObject = theTrackable.gameObject;
        //disable any pre-existing augmentation
        for (int i = 0; i < trackableGameObject.transform.childCount; i++)
        {
            Transform child = trackableGameObject.transform.GetChild(i);
            child.gameObject.SetActive(false);
            //child.gameObject.active = false;
        }
        // Create a simple cube object
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Re-parent the cube as child of the trackable gameObject
        cube.transform.parent = theTrackable.transform;
        // Adjust the position and scale
        // so that it fits nicely on the target
        cube.transform.localPosition = new Vector3(0, 0.2f, 0);
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // Make sure it is active
        cube.SetActive(true);
        //cube.active = true;
    }
}