using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Time.timeScale = 1;
        player.transform.position = levelSpawns[PlayerPrefs.GetInt("ActualLevel")].transform.position;
    }

    public void ResetGame()
    {
        PlayerPrefs.SetInt("ActualLevel", 0);
        SceneManager.LoadScene(0);
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }



}
