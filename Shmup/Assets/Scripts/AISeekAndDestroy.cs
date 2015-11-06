using UnityEngine;
using System.Collections;

public class AISeekAndDestroy : MonoBehaviour {

    bool inPlay;
    public Transform player;
    public float chaseSpeed;

	// Use this for initialization
	void Start () {
        inPlay = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (inPlay)
        {
            Vector3 targetDir = player.position - transform.position;
            float step = chaseSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boundary")
            inPlay = true;
    }
}
