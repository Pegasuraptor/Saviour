using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour 
{
    void OnTriggerExit(Collider other)
    {
        //if(other.gameObject.tag == "Player")
        //{
        //    other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, this.transform.position.z);
        //}
        //else
        //{
           Destroy(other.gameObject);
        //}
    }
}
