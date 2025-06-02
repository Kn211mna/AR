using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    if (spawnedObject == null)
                        spawnedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                    else
                        spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
}
