using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelObjective : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    int currentSceneIndex;
    int nextSceneIndex;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex + 1;
    }

    void Update()
    {
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // If there is no next scene start at first level
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        if (this.transform.childCount == 0)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
