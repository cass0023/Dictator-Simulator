using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The main class for controlling the different interactions between game mechanics. Gamemanager is an instance that doesn't use monobehavior
/// </summary>
public class GameManager
{
	private static GameManager instance = new GameManager();

    private GameManager()
    {
        // initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
        // because the game manager will be created before the objects
    }

	public static GameManager Instance 
    {
        get { return instance; }
    }

	//Initialize stuff here

	public int WeekNum = 0;

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
        EventManager.Instance.UpdateEventState();
        WeekNum++;

        Debug.Log($"Changed Week to week {WeekNum}");
    }

}
