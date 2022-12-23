using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraShake : MonoBehaviour
{

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originPos = transform.localPosition;
        float elased = 0.0f;
        while (elased < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            //  float z = Random.Range(.1f, .16f) * magnitude;

            transform.localPosition = new Vector3(originPos.x + x, originPos.y, originPos.z);

            elased += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originPos;
    }
}
