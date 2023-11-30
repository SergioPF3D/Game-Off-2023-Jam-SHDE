using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class ScalableObject : MonoBehaviour
{
    [Header("ScalableObject")]

    public Color emisiveColor;
    public float baseEmisiveIntensity;

    public int layer;

    //Return to initial position
    Vector3 basePosition;

    [SerializeField]
    float timeToRespawn;

    float timePassed;

    [Space(20)]

    [SerializeField]
    AudioSource audiosou;

    [SerializeField]
    AudioClip respawn;

    public virtual void Start()
    {
        basePosition = transform.position;

        layer = gameObject.layer;
        this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmisiveColor",emisiveColor);
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", emisiveColor);
        baseEmisiveIntensity = this.gameObject.GetComponent<MeshRenderer>().material.GetFloat("_EmisiveIntensity");

        ChangeMass();

    }
    public virtual void ChangeMass()
    {
        this.GetComponent<Rigidbody>().mass = (this.transform.localScale.x + this.transform.localScale.y + this.transform.localScale.z) / 3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Podria setear el rigidbody en este script y no en los otros
        if (this.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            PlayCollision();
        }
        
    }

    public IEnumerator ResPawn()
    {
        
        while (timePassed < timeToRespawn)
        {
            timePassed += Time.fixedDeltaTime;

            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_OutlineWidth", 0.1f);
            gameObject.layer = layer;
            GameObject.Find("Player").GetComponentInChildren<MoveObjects>().outlined = null;
            //outlined = null;

            gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DissolveAmount", timePassed / timeToRespawn);
            yield return new WaitForFixedUpdate();
        }

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = basePosition;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_DissolveAmount", 0);
        timePassed = 0;

        audiosou.PlayOneShot(respawn, 2);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            StartCoroutine(ResPawn());
        }
    }

    void PlayCollision()
    {
        audiosou.pitch = 1 + Random.Range(-0.1f, 0.1f);
        //audiosou.volume Que dependa de la velocidad
        audiosou.Play();

    }
}


