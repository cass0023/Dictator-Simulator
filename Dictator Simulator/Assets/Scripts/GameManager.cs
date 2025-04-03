using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public void LoadEvents()
    {
		EventManager.Instance.UpdateEventState(); //Check if any events need to be unlocked.
		LoadStaticEvents<EmailEvent, ScriptableEvent>();
		LoadStaticEvents<SocialEvent, ScriptableSocialMedia>();
		LoadStaticEvents<NewsEvent, ScriptableNews>();
		LoadStaticEvents<OrderEvent, ScriptableOrder>();

		BackgroundManager.Instance.CheckSwitchBackground(0);
	}

    /// <summary>
    /// Handle moving to the next week. Change stats as necissary.
    /// </summary>
    public void GoToNextWeek()
    {
		for (int i = 1; i <= 4; i++)
        {
            StatManager.Instance.UpdateText((Stats)i);
            StatManager.Instance.UpdateSliders((Stats)i);
        }
        //Update each event type
		WeekNum++;
        TextMeshProUGUI calenderText = GameObject.Find("T_WeekNum").GetComponent<TextMeshProUGUI>();
        calenderText.text = "" + WeekNum;
        Debug.Log($"Changed Week to week {WeekNum}");
		
		LoadEvents();
		BackgroundManager.Instance.CheckSwitchBackground(StatManager.Instance.GetStatValue(Stats.FEAR));
    }

	/// <summary>
	/// Load the first event for whatever type T is that is not the default, is unlocked, and is not completed.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="U"></typeparam>
    public void LoadStaticEvents<T,U>() where T : class, IIsEvent<U> where U : IUnlockable
	{
		T nextEvent = EventManager.Instance.GetNextEvent<T, U>();

		if (nextEvent == null)
		{
			Debug.LogWarning($"No available event found for type {typeof(T).Name}");
			return;
		}

		if (typeof(T) == typeof(EmailEvent))
		{
			EmailManager.Instance.InitializeEmail(nextEvent as EmailEvent);
		}
		else if (typeof(T) == typeof(SocialEvent))
		{
			SocMediaManager.Instance.InitializeSocial(nextEvent as SocialEvent);
		}
		else if (typeof(T) == typeof(NewsEvent))
		{
			NewsManager.Instance.InitializeNews(nextEvent as NewsEvent);
		}
        //copy and pasted the other if statement and changed it to order var
        else if (typeof(T) ==typeof(OrderEvent))
		{
			OrderManager.Instance.InitializeOrder(nextEvent as OrderEvent);
		}
		else
		{
			Debug.LogWarning($"Unhandled event type: {typeof(T).Name}");
		}
	}

}
