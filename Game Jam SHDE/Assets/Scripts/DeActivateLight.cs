using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeActivateLight : MonoBehaviour
{
    [SerializeField]
    float timeToStart;

    [SerializeField]
    float timeToDeactivate;


    [SerializeField]
    Light lighttofade;

    void Start()
    {
        if (PlayerPrefs.HasKey("Light"))
        {
            if (PlayerPrefs.GetFloat("Light") == 0)
            {
                lighttofade.gameObject.SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("Light", lighttofade.intensity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FadeLight(lighttofade, timeToDeactivate, timeToStart));
        Debug.Log(1);
    }

    IEnumerator FadeLight(Light _lighttofade, float _timeToFade, float timeToStart)
    {
        float timePassed = 0;
        float startIntensity = _lighttofade.intensity;
        yield return new WaitForSeconds(timeToStart);
        while (timePassed / _timeToFade < 1)
        {
            timePassed += Time.fixedDeltaTime;
            _lighttofade.intensity = Mathf.Lerp(startIntensity, 0, timePassed / _timeToFade);
            yield return new WaitForFixedUpdate();
        }
        _lighttofade.gameObject.SetActive(false);
        PlayerPrefs.SetFloat("Light", 0);
    }
}
