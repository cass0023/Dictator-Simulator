using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The main class for controlling the different interactions between game mechanics.
/// </summary>
public class GameManager : MonoBehaviour
{

    public int WeekNum = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Load a new scene using the name. It must match exactly.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"Invalid scene name {0}. Check your spelling.");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// Handle moving to the next week. Change stats as necissary.
    /// </summary>
    public void GoToNextWeek()
    {
        WeekNum++;
    }

}
