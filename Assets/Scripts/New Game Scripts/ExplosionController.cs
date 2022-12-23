using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    float force = 800;
    public void Explosion()
    {
        Vector2 expPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(expPos, 10f);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null && rb.transform.tag != "Press")
            {
                Debug.Log(rb.transform.name);
                rb.AddExplosionForce(-force*20f, expPos, 5f, 50, ForceMode.Impulse);
            }
        }
    }
}
