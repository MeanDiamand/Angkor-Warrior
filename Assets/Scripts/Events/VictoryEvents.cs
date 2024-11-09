using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryEvents : MonoBehaviour
{
    public GameObject victoryPanel;
    AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Time.timeScale = 0;
            victoryPanel.SetActive(true);
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            audioManager.PlaySFX(audioManager.victory);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        victoryPanel.SetActive(false);

        // Loading the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        victoryPanel.SetActive(false);

        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the scene name ends with a number
        if (int.TryParse(currentSceneName.Substring(currentSceneName.Length - 1), out int levelNumber))
        {
            // Increment the level number
            levelNumber++;

            // Construct the next level scene name
            string nextSceneName = "Level" + levelNumber;

            // Load the next level scene
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Current scene name does not follow the 'LevelX' format.");
        }
    }

    public void Home()
    {
        victoryPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
