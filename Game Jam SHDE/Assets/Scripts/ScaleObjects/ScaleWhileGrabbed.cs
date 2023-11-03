using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWhileGrabbed : MonoBehaviour
{
    Vector3 baseScale;
    [SerializeField]
    Vector3 maxScale;

    [SerializeField]
    float maxTime;
    float actualTime;

    float t;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        StopCoroutine("DeEscalate");
    }
    private void OnMouseDrag()
    {
        actualTime += Time.fixedDeltaTime;


        t = maxTime / actualTime;
        Debug.Log(t + "    " + actualTime);
        
        transform.localScale = Vector3.Lerp(baseScale, maxScale, t);
    }
    private void OnMouseUp()
    {
        StartCoroutine("DeEscalate");
    }
    IEnumerator DeEscalate()
    {
        actualTime = 0;

        while (t < 1)
        {
            actualTime += Time.deltaTime;

            t = actualTime * 1 / maxTime;

            transform.localScale = Vector3.Lerp(maxScale, baseScale, t);
            yield return Time.deltaTime;
        }
        actualTime = 0;
        yield return null;
    }
}
