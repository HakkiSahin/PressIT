using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class TestObject : MonoBehaviour
{
    public float speed = 1;
    public ObiRopeCursor cursor;
    public ObiRope rope;
    public MeshFilter obiMesh;
    bool isTouch;
    Rigidbody myRigid;

    [SerializeField] List<MeshFilter> _meshFilters;

    void Start()
    {
        //cursor = GetComponentInChildren<ObiRopeCursor>();
        //rope = cursor.GetComponent<ObiRope>();
        myRigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            isTouch = true;
            cursor.ChangeLength(rope.restLength + 3 * Time.deltaTime);
            this.transform.parent.GetChild(2).GetComponent<Rigidbody>().isKinematic = false;
            this.transform.parent.GetChild(2).GetComponent<Rigidbody>().AddForce(new Vector3(-0.2f, 1f, 0) , ForceMode.Force);
            //  myRigid.AddForce(new Vector3(-0.2f, 1f, 0) , ForceMode.Impulse);
            // transform.Translate(new Vector3(-0.02f, -1f, 0) * Time.deltaTime);
        }


        if (rope.restLength > 2)
        {
          //  this.transform.GetComponent<Rigidbody>().isKinematic = false;
            //transform.Translate(new Vector3(-0.02f, -1f, 0) * Time.deltaTime);
            // cursor.ChangeLength(rope.restLength + 3 * Time.deltaTime);
            
        }


        for (int i = 0; i < _meshFilters.Count; i++)
        {
            _meshFilters[i].mesh = obiMesh.mesh;
        }
    }


    IEnumerator AddLenght()
    {
        yield return new WaitForSeconds(0.3f);
        // myRigid.useGravity = true;
    }
}