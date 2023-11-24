using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    [SerializeField]
    int level;

    [SerializeField]
    Door door;

    [SerializeField]
    AudioSource audios;
    private void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("ActualLevel", level);
        door.blocked = false;
        door.DeActivate();
        door.blocked = true;
    }
    private void OnTriggerExit(Collider other)
    {
        audios.Play();
    }
}
