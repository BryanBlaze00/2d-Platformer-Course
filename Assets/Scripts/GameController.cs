using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] readonly float levelLoadDelay = 1f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;
    // PlayerController playerController;
    int currentSceneIndex;
    bool gameover = false;

    void Awake()
    {
        // Only one!
        int numGameControllers = FindObjectsOfType<GameController>().Length;
        if (numGameControllers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        // playerController = GetComponent<PlayerController>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessGainPoints(int value)
    {
        if (playerLives > 0)
        {
            score += value;
            scoreText.text = score.ToString();
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        StartCoroutine(LoadLevel(currentSceneIndex));
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        StartCoroutine(LoadLevel(0));
        gameover = true;
    }

    IEnumerator LoadLevel(int level)
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        if (gameover)
            Destroy(gameObject);
        SceneManager.LoadScene(level);
    }
}
