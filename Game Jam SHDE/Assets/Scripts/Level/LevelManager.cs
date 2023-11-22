using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> levelSpawns;

    [SerializeField]
    Transform player;
    void Start()
    {
        //Set Player
        //player.transform.position = levelSpawns[PlayerPrefs.GetInt("ActualLevel")].transform.position;
    }

    public void ResetGame()
    {
        player.transform.position = levelSpawns[0].transform.position;
        PlayerPrefs.SetInt("ActualLevel", 0);
    }
    public void ResetLevel()
    {
        player.transform.position = levelSpawns[PlayerPrefs.GetInt("ActualLevel")].transform.position;
    }



}
