using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tesssst : MonoBehaviour
{
    Rigidbody myRigid;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myRigid.AddForce(new Vector3(0.2f, 1f, 0) * 5f, ForceMode.Impulse);
            myRigid.useGravity = true;
        }
    }
}
