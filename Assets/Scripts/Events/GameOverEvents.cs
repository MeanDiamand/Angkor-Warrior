using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverEvents : MonoBehaviour
{
    public static bool isGameOver;
    public GameObject gameOverPanel;

    private void Awake()
    {
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver)
        {
            gameOverPanel.SetActive(true);
            isGameOver=false;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);

        // Loading the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
