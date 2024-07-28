using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteableObject : MonoBehaviour
{
    Grabbable grabbable;


    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponentInChildren<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("SelectingPointsCount - " + ovrGrabbable.SelectingPointsCount); // points on object that are grabbing
        // Debug.Log("PointsCount - " + ovrGrabbable.PointsCount); // points on object
        if (grabbable.SelectingPointsCount > 0)
        {
            if (OVRInput.GetUp(OVRInput.RawButton.X) || OVRInput.GetUp(OVRInput.RawButton.B)) Destroy(gameObject);
        }
    }
}
