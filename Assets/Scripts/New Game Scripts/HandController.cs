using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{


    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator HandSimulate(Quaternion a, Quaternion b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.rotation = Quaternion.Lerp(a, b, i);
            yield return null;
        }
    }
}
