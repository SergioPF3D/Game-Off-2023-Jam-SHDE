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
    void Awake()
    {
        //Set Player
        Time.timeScale = 1;
        player.transform.position = levelSpawns[PlayerPrefs.GetInt("ActualLevel")].transform.position;
        if (PlayerPrefs.GetInt("ActualLevel") == 0)
        {
            //PlayerPrefs.SetFloat("Light", lighttofade.intensity);
            PlayerPrefs.DeleteKey("Light");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            if (Input.GetKey(KeyCode.Alpha0))
            {
                player.transform.position = levelSpawns[0].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                player.transform.position = levelSpawns[1].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                player.transform.position = levelSpawns[2].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                player.transform.position = levelSpawns[3].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                player.transform.position = levelSpawns[4].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                player.transform.position = levelSpawns[5].transform.position;

            }
            if (Input.GetKey(KeyCode.Alpha6))
            {
                player.transform.position = levelSpawns[6].transform.position;
            }
        }
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
