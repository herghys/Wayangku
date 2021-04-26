using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] float rot = 20;

    void Update()
    {
        if (1 == Input.touchCount)
        {
            gameObject.transform.Rotate(new Vector3(0,-1,0), Input.touches[0].deltaPosition.x * rot * Time.deltaTime, Space.World); ;
            //gameObject.transform.Rotate(Vector3.right, Input.touches[0].deltaPosition.y * rot  * Time.deltaTime);
        }
    }
}