using UnityEngine;
using System.Collections;

public class HomingShot : MonoBehaviour {

    public float chaseSpeed;

    private GameObject hazardContainer;
    private Transform hazardToTrack;
    private bool lockedOn;
    private bool seeking;

	// Use this for initialization
	void Start () {
        hazardContainer = GameObject.Find("HazardContainer");

        if(!hazardContainer)
        {
            Debug.Log("MissileGuidance cannot find HazardContainer script.");
        }

        lockedOn = false;
        seeking = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(!seeking && !lockedOn)
        {
            seeking = true;
            lockedOn = false;
            SeekHazard();
        }

	    if(lockedOn)
        {
            if(!hazardToTrack)
            {
                //If hazard is destroyed, the missile will just fly off in the direction it is facing.
                GetComponent<Rigidbody>().velocity = transform.forward * chaseSpeed;
                return;
            }

            Vector3 targetDir = hazardToTrack.position - transform.position;
            float step = chaseSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.position = Vector3.MoveTowards(transform.position, hazardToTrack.position, chaseSpeed * Time.deltaTime);
        }
	}

    void SeekHazard()
    {
        if(hazardContainer.transform.childCount == 0)
        {
            //Should never happen but it's safer to be sure.
            seeking = false;
            lockedOn = false;
            return;
        }

        float currentClosestDistance = Vector3.Distance(transform.position, hazardContainer.transform.GetChild(0).position);
        float nextDistance; 

        foreach(Transform child in hazardContainer.transform)
        {
            nextDistance = Vector3.Distance(transform.position, child.position);
            if(nextDistance <= currentClosestDistance)
            {
                currentClosestDistance = nextDistance;
                hazardToTrack = child;
            }
        }

        lockedOn = true;
        seeking = false;
    }
}
