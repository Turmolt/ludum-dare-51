using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }
    public void LoadNextLevel()
    {
        var currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
        
        if (SceneManager.sceneCountInBuildSettings <= currentSceneNumber + 1) return;

        SceneManager.LoadScene(currentSceneNumber + 1);
    }
}
