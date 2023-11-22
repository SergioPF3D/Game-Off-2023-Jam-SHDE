using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    [SerializeField]
    int level;

    [SerializeField]
    Door door;
    private void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetInt("ActualLevel", level);
        door.DeActivate();
        door.blocked = true;
    }
}
