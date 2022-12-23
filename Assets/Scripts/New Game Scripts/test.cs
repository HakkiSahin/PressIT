using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject conveyorMat;
    public Transform spawnPoint, parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rail")
        {
            GameObject obj = Instantiate(conveyorMat, spawnPoint.position, Quaternion.identity, parent);
            obj.transform.rotation = spawnPoint.rotation;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        Destroy(other.gameObject);

    }
}
