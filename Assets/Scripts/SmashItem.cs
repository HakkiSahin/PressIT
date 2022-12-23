using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Deform;

public class SmashItem : MonoBehaviour
{
    private bool isCrash, isFail;
    public bool isParticle;
    [SerializeField] float maxDeform, deformSpeed;
    [SerializeField] GameObject plate;
    [SerializeField] BulgeDeformer deformer;


    float mass = 0;

    private void Start()
    {
        if (this.transform.tag == "Rare")
        {
            mass = 10f;
        }
    }

    void Update()
    {
        if (isCrash)
        {
            ScaleChanged();
        }
        else if (isFail)
        {
            FailChange();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Press")
        {
            Debug.Log(other.name);
            if (other.GetComponent<Rigidbody>().mass > mass)
            {
                isCrash = true;
                if (this.transform.tag == "Smash")
                {
                    other.transform.parent.GetComponent<PressController>().CreateJellyEffect();
                }
            }
            else
            {
                other.transform.parent.GetComponent<PressController>().FailCrash();
                other.transform.parent.GetComponent<PressController>().isTouch = true;
                isFail = true;
                mass = other.GetComponent<Rigidbody>().mass;
            }
        }

        else if (other.tag == "SideFace")
        {
            this.gameObject.AddComponent<Rigidbody>();
            // this.gameObject.transform.parent = null;
            this.transform.GetComponent<Rigidbody>()
                .AddForce(new Vector3(Random.Range(-1f, 0f), 1, Random.Range(0.5f, 1)) * 4f, ForceMode.Impulse);

            plate.gameObject.AddComponent<Rigidbody>();
            plate.gameObject.transform.parent = null;
            plate.transform.GetComponent<Rigidbody>()
                .AddForce(new Vector3(Random.Range(-1f, 0f), 1, Random.Range(-1f, 1f)) * 7f, ForceMode.Impulse);
            plate.transform.GetComponent<BoxCollider>().isTrigger = true;
            Destroy(plate, 3f);

            //   this.transform.parent.gameObject.AddComponent<Rigidbody>();
            //   this.transform.parent.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //   other.GetComponent<ExplosionController>().Explosion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Press")
        {
            isCrash = false;
        }
    }

    void ScaleChanged()
    {
        #region First Type

        //switch (type)
        //{
        //    case DeformType.posX:
        //        if (transform.localScale.x > maxDeform)
        //        {
        //            this.transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 8f, transform.localScale.y + Time.deltaTime * 14f, transform.localScale.z + Time.deltaTime * 14f);
        //        }
        //        else
        //        {
        //            gameObject.transform.parent.GetComponent<DestroyController>().ChangeDestroyObject();
        //        }
        //        break;
        //    case DeformType.posY:
        //        if (transform.localScale.y > maxDeform)
        //        {
        //            this.transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 14f, transform.localScale.y - Time.deltaTime * 8f, transform.localScale.z + Time.deltaTime * 14f);
        //        }
        //        else
        //        {
        //            gameObject.transform.parent.GetComponent<DestroyController>().ChangeDestroyObject();
        //        }
        //        break;
        //    case DeformType.posZ:
        //        if (transform.localScale.z > maxDeform)
        //        {
        //            this.transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 14f, transform.localScale.y + Time.deltaTime * 14f, transform.localScale.z - Time.deltaTime * 8f);
        //        }
        //        else
        //        {
        //            gameObject.transform.parent.GetComponent<DestroyController>().ChangeDestroyObject();
        //        }
        //        break;
        //    default:
        //        break;
        //}

        #endregion

        if (deformer.Factor < maxDeform)
        {
            deformer.Factor += Time.deltaTime * deformSpeed;
        }
        else
        {
            this.transform.parent.GetComponent<DestroyController>().ChangeDestroyObject();
        }
    }

    void FailChange()
    {
        if (deformer.Factor < maxDeform)
        {
            deformer.Factor += Time.deltaTime * deformSpeed;
        }
    }
}