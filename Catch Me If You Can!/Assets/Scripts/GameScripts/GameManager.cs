using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("GameStartScene");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("GameStartScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
