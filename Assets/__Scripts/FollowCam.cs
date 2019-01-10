using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {
    static public GameObject POI; //The static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; //The desired Z pos of the camera

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        /*
        if (POI == null) return; //return if there is no poi

        //Get the position of the POI
        Vector3 destination = POI.transform.position;
         */

        Vector3 destination;
        // If there is no POI, return to P:[0, 0, 0]
        if (POI == null) {
            destination = Vector3.zero;
        } else {
            destination = POI.transform.position;
            // if POI is a Projectile and it's sleeping
            if (POI.tag == "Projectile") {
                if (POI.GetComponent<Rigidbody>().IsSleeping()) {
                    // return to default view
                    POI = null;
                    // in the next update
                    return;
                }
            }
        }

        // Limit the X & Y to minimum values
        // X - prevent Camera Following to the left side (not game zone)
        // Y - prevent unnecessary zoom for the destination (projectile)
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        //Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;
        //Set the camera to the destination
        transform.position = destination;
        //Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
