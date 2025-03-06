using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class EmailEvent : IIsEvent<ScriptableEvent>
{
	public ScriptableEvent Data { get; set; }
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}

public class SocialEvent : IIsEvent<ScriptableSocialMedia>
{
	public ScriptableSocialMedia Data { get; set; }
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}

public interface IIsEvent<T>
{ 
	T Data { get; set; }
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}


public class EventManager
{
	public EmailEvent[] EmailEvents;
	public SocialEvent[] SocialEvents;
	private AllEventsContainer AllEvents;
	bool Loaded = false;

	private static EventManager instance = new EventManager();
	private EventManager()
	{

	}

	public static EventManager Instance
	{
		get { return instance; }
	}

	private void LoadEvents()
	{
		AllEvents = GameObject.Find("AllEventsContainer").GetComponent<AllEventsContainer>();
		EmailEvents = new EmailEvent[AllEvents.Emails.Length];
		SocialEvents = new SocialEvent[AllEvents.SocialMedia.Length];
		//Load Emails
		for (int i = 0; i < AllEvents.Emails.Length; i++)
		{
			EmailEvent emailEvent = new EmailEvent();
			emailEvent.Data = AllEvents.Emails[i];
			EmailEvents[i] = emailEvent;
			emailEvent.HasBeenUnlockedByEvent = false;
		}
		//Load social media posts
		for (int i = 0; i < AllEvents.SocialMedia.Length; i++)
		{
			SocialEvent socialEvent = new SocialEvent();
			socialEvent.Data = AllEvents.SocialMedia[i];
			SocialEvents[i] = socialEvent;
			socialEvent.HasBeenUnlockedByEvent = false;
		}


		Loaded = true;
		Debug.Log("Loaded all events.");
	}

	/// <summary>
	/// Call this to update events and check if they should be locked / unlocked.
	/// </summary>
	public void UpdateEventState()
	{
		if(!Loaded) { LoadEvents(); }

		if (EmailEvents != null)
		{
			for (int i = 1; i < EmailEvents.Length; i++) //Start at 1 because of empty email
			{
				EmailEvents[i].IsUnlocked = CheckUnlock<EmailEvent, ScriptableEvent>(EmailEvents[i]);
			}
		}
<<<<<<< Updated upstream
=======

		if (SocialEvents != null)
		{
			for (int i = 1; i < SocialEvents.Length; i++) //Start at 1 because of empty email
			{
				SocialEvents[i].IsUnlocked = CheckUnlock<SocialEvent, ScriptableSocialMedia>(SocialEvents[i]);
			}
		}
>>>>>>> Stashed changes
	}

	/// <summary>
	/// Checks if the current event is unlocked or not. Compares the current player stats to the specified value when the event was created.
	/// Also checks to see if an event is unlocked by another event.
	/// </summary>
	/// <param name="curEvent"></param>
	/// <returns></returns>
	private bool CheckUnlock<T, U>(T curEvent) where T : IIsEvent<U> where U:IUnlockable
	{
		bool shouldUnlock = true;

		if (curEvent.Data.LockedByOtherEvent) //Get the property with the name LockedByOtherEvent in Data class
		{
			shouldUnlock &= curEvent.HasBeenUnlockedByEvent;
		}
		
		if(curEvent.Data.StatLocks.Length > 0)
		{
			foreach (UnlockEventByStatData e in curEvent.Data.StatLocks)
			{
				switch (e.RequiredStatVal)
				{
					case (RequiredStatVal.EQUAL): //Note: Must be exactly equal, which can be difficult for floats.
						shouldUnlock &= (StatManager.Instance.GetStatValue(e.lockType) == e.StatVal);
						break;
					case (RequiredStatVal.ABOVE):
						shouldUnlock &= (StatManager.Instance.GetStatValue(e.lockType) >= e.StatVal);
						break;
					case (RequiredStatVal.BELOW):
						shouldUnlock &= (StatManager.Instance.GetStatValue(e.lockType) <= e.StatVal);
						break;
					default:
						Debug.LogError($"Error reading data {e}");
						break;
				}
			}
		}
		if (curEvent.Data.WeekLocks.Length > 0)
		{
			foreach (UnlockEventByWeekData e in curEvent.Data.WeekLocks)
			{
				switch (e.RequiredStatVal)
				{
					case (RequiredStatVal.EQUAL): //Note: Must be exactly equal, which can be difficult for floats.
						shouldUnlock &= (GameManager.Instance.WeekNum == e.WeekNum);
						break;
					case (RequiredStatVal.ABOVE):
						shouldUnlock &= (GameManager.Instance.WeekNum >= e.WeekNum);
						break;
					case (RequiredStatVal.BELOW):
						shouldUnlock &= (GameManager.Instance.WeekNum <= e.WeekNum);
						break;
					default:
						Debug.LogError($"Error reading data {e}");
						break;
				}
			}
		} 
		if (curEvent.IsCompleted)
		{
			Debug.Log($"{curEvent.Data.EventName} is [COMPLETED]");
		}
		else if (shouldUnlock)
		{
			Debug.Log($"{curEvent.Data.EventName} is [UNLOCKED]");
		}
		else
		{
			Debug.Log($"{curEvent.Data.EventName} is [LOCKED]");
		}

		return shouldUnlock;
	}
	/// <summary>
	/// Get the event with a certain name, regardless if it is unlocked or not. If the name does not exits, return null.
	/// </summary>
	/// <param name="EventName"></param>
	/// <returns></returns>
	public T GetEvent<T, U>(string EventName) where T : IIsEvent<U>
	{
		if (typeof(T).Equals(typeof(EmailEvent)))
		{
			for (int i = 1; i < EmailEvents.Length; i++)
			{
				if (EmailEvents[i].Data.EventName == EventName)
				{
					return (T)EmailEvents[i].ConvertTo(typeof(T));
				}
			}

			Debug.LogError($"{EventName} does not exist in the list of events. Returning empty inbox event.");
			return (T)EmailEvents[0].ConvertTo(typeof(T));
		}
		else if (typeof(T).Equals(typeof(SocialEvent)))
		{
			for (int i = 1; i < SocialEvents.Length; i++)
			{
				if (SocialEvents[i].Data.EventName == EventName)
				{
					return (T)SocialEvents[i].ConvertTo(typeof(T));
				}
			}

			Debug.LogError($"{EventName} does not exist in the list of events. Returning empty social post event.");
			return (T)SocialEvents[0].ConvertTo(typeof(T));
		}
		else
		{
			Debug.LogError($"Type {typeof(T)} does not exist as a type of event that can be retrieved.");
			return default;
		}

	}

	/// <summary>
	/// Return a random unlocked event of type T. No error checking.
	/// </summary>
	/// <returns></returns>
	public EmailEvent GetRandomEvent()
	{
		List<EmailEvent> unlocked_Events = new List<EmailEvent>();
		for (int i = 0; i < EmailEvents.Length; i++)
		{
			if(EmailEvents[i].IsUnlocked && !EmailEvents[i].IsCompleted)
			{
				unlocked_Events.Add(EmailEvents[i]);
			}
		}


			if (unlocked_Events.Count < 1)
			{
				GameObject.Find("NotificationBubble").SetActive(false);
				return (T)EmailEvents[0].ConvertTo(typeof(T));
			}
			else if (unlocked_Events.Count > 1)
			{
				GameObject.Find("NotificationBubble").SetActive(true);
			}

			return (T)unlocked_Events[UnityEngine.Random.Range(0, unlocked_Events.Count)].ConvertTo(typeof(T));
		}
		else if (typeof(T).Equals(typeof(SocialEvent)))
		{
			List<SocialEvent> unlocked_Events = new List<SocialEvent>();
			for (int i = 0; i < SocialEvents.Length; i++)
			{
				if (SocialEvents[i].IsUnlocked && !SocialEvents[i].IsCompleted)
				{
					unlocked_Events.Add(SocialEvents[i]);
				}
			}


			if (unlocked_Events.Count < 1)
			{
				return (T)SocialEvents[0].ConvertTo(typeof(T));
			}
			
			return (T)unlocked_Events[UnityEngine.Random.Range(0, unlocked_Events.Count)].ConvertTo(typeof(T));
		}
		else
		{
			Debug.LogError($"Invalid event type {typeof(T)}");
			if(typeof(T).Equals(typeof(EmailEvent))) 
			{
				return (T)EmailEvents[0].ConvertTo(typeof(T));
			}
			else
			{
				return (T)SocialEvents[0].ConvertTo(typeof(T));
			}
		}
<<<<<<< Updated upstream

		if(unlocked_Events.Count < 1)
		{
            GameObject.Find("NotificationBubble").SetActive(false);
            return EmailEvents[0];
		}
<<<<<<< HEAD
		else if (unlocked_Events.Count > 1)
		{
            GameObject.Find("NotificationBubble").SetActive(true);
        }

        return unlocked_Events[UnityEngine.Random.Range(0, unlocked_Events.Count)];
=======
	
		return unlocked_Events[UnityEngine.Random.Range(0, unlocked_Events.Count)];
>>>>>>> parent of db8134a (Revert "Merge branch 'main' into Chris-WorkingBranch")
	}

	public void CompleteEvent(string EventName) 
	{
		for (int i = 1; i < EmailEvents.Length; i++)
		{
			if (EmailEvents[i].Data.EventName == EventName)
			{
				EmailEvents[i].IsCompleted = true;
				Debug.Log($"Set {EmailEvents[i].Data.EventName} to completed.");
			}
		}
		for (int i = 1; i < SocialEvents.Length; i++)
		{
			if (SocialEvents[i].Data.EventName == EventName)
			{
				SocialEvents[i].IsCompleted = true;
				Debug.Log($"Set {SocialEvents[i].Data.EventName} to completed.");
			}
		}

	}
}


//All of the different types of events and their associated variables.

public class IncreaseStatEventArgs : EventArgs
{
	public Stats StatToIncrease;
	public float Amount;
}

public class DecreaseStatEventArgs : EventArgs
{
	public Stats StatToDecrease;
	public float Amount;
}