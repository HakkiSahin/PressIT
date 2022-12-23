using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class Oniropetest : MonoBehaviour
{
    [SerializeField] List<MeshFilter> _meshs;
    [SerializeField] private SkinnedMeshRenderer _mesh;
    [SerializeField] private GameObject _parent;

    void Start()
    {
        StartCoroutine(Test());
    }

    void Update()
    {
        for (int i = 0; i < _meshs.Count; i++)
        {
            _mesh.BakeMesh(_meshs[i].mesh);
        }
    }

    IEnumerator Test()
    {
        float explosionTime = 1.0f;
        float explosionRadius = 3.0f;
        float explosionPower = 500.0f;
        _mesh.GetComponent<ObiSoftbody>().shapeMatchingConstraintsEnabled = true;

        for (int i = 0; i < _meshs.Count; i++)
        {
            _meshs[i].GetComponent<MeshRenderer>().enabled = true;
        }

        yield return new WaitForSeconds(.5f);

        _mesh.GetComponent<ObiParticleAttachment>().enabled = false;
        for (int i = 0; i < _meshs.Count; i++)
        {
            Rigidbody myRigid = _meshs[i].gameObject.AddComponent<Rigidbody>();
        }

        Collider[] objects = UnityEngine.Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach (Collider h in objects)
        {
            Rigidbody r = h.GetComponent<Rigidbody>();
            if (r != null && r.transform.tag == "Fly")
            {
                r.AddExplosionForce(explosionPower, this.transform.position, explosionRadius);
            }
        }

        Destroy(_parent, 2);
    }
}