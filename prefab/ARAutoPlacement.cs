using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARAutoPlacement : MonoBehaviour
{
    public GameObject assetPrefab;

    private ARRaycastManager raycastManager;
    private GameObject spawnedAsset;

    void Start()
    {
        raycastManager = FindAnyObjectByType<ARRaycastManager>();
    }

    void Update()
    {
        // Don't place the dog again once it has been placed.
        if (spawnedAsset != null)
            return;

        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Use the center of the phone screen.
        Vector2 screenCenter = new Vector2(
            Screen.width / 2f,
            Screen.height / 2f
        );

        // Look for a real-world horizontal plane.
        if (raycastManager.Raycast(
                screenCenter,
                hits,
                TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Make the dog face the camera.
            Vector3 directionToCamera =
                Camera.main.transform.position - hitPose.position;

            directionToCamera.y = 0f;

            Quaternion rotation;

            if (directionToCamera.sqrMagnitude > 0.001f)
            {
                rotation = Quaternion.LookRotation(directionToCamera);
            }
            else
            {
                rotation = hitPose.rotation;
            }

            // Place the dog ON the detected plane.
            spawnedAsset = Instantiate(
                assetPrefab,
                hitPose.position,
                rotation
            );
        }
    }
}