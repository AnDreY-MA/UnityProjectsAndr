using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Load()
    {
        player.LoadPlayer();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
