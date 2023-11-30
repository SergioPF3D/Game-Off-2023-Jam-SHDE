using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour 
{
    [SerializeField]
    float MinLightIntensity = 0.6F;
    [SerializeField]
    float MaxLightIntensity = 1.0F;

    
    
    [SerializeField]
    float AccelerateTime = 0.15f;

    private float _targetIntensity = 1.0f;
    private float _lastIntensity = 1.0f;

    private float _timePassed = 0.0f;

    [SerializeField]
    Light _lt;
    private const double Tolerance = 0.0001;




    [SerializeField]
    float intensityVariationPercentaje;

    [SerializeField]
    float baseIntensity;

    private void Start()
    {
        //_lt = GetComponent<Light>();
        _lastIntensity = _lt.intensity;
        FixedUpdate();

        baseIntensity = _lt.intensity;
    }

    private void FixedUpdate()
    {
        
        _timePassed += Time.deltaTime;
        _lt.intensity = Mathf.Lerp(_lastIntensity, _targetIntensity, _timePassed / AccelerateTime);

        if (Mathf.Abs(_lt.intensity - _targetIntensity) < Tolerance)
        {
            _lastIntensity = _lt.intensity;
            _targetIntensity = Random.Range(MinLightIntensity, MaxLightIntensity);
            _targetIntensity = Random.Range(MinLightIntensity, MaxLightIntensity);
            _timePassed = 0.0f;


               //_lt.intensity = Random.Range(baseIntensity - baseIntensity * intensityVariationPercentaje / 100, baseIntensity + baseIntensity * intensityVariationPercentaje / 100);

            //intensityVariationPercentaje
        }
        


    }
}
