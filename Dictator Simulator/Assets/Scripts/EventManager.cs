using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class EmailEvent
{
	public ScriptableEvent Data;
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}

public class EventManager
{
	public EmailEvent[] EmailEvents;
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
		for (int i = 0; i < AllEvents.Emails.Length; i++)
		{
			EmailEvent emailEvent = new EmailEvent();
			emailEvent.Data = AllEvents.Emails[i];
			EmailEvents[i] = emailEvent;
			emailEvent.HasBeenUnlockedByEvent = false;
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
				EmailEvents[i].IsUnlocked = CheckUnlock(EmailEvents[i]);
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
	private bool CheckUnlock(EmailEvent curEvent)
	{
		bool shouldUnlock = true;

		if(curEvent.Data.LockedByOtherEvent) //If the event is locked by another event
		{
			//Check if this event has already been unlocked by the other event
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
			Debug.Log($"{curEvent.Data.name} is [COMPLETED]");
		}
		else if (shouldUnlock)
		{
			Debug.Log($"{curEvent.Data.name} is [UNLOCKED]");
		}
		else
		{
			Debug.Log($"{curEvent.Data.name} is [LOCKED]");
		}

		return shouldUnlock;
	}
	/// <summary>
	/// Get the event with a certain name, regardless if it is unlocked or not. If the name does not exits, return null.
	/// </summary>
	/// <param name="EventName"></param>
	/// <returns></returns>
	public EmailEvent GetEvent(string EventName)
	{
		for(int i = 1; i < EmailEvents.Length; i++) 
		{
			if (EmailEvents[i].Data.EventName == EventName)
			{
				return EmailEvents[i];
			}
		}

		Debug.LogError($"{EventName} does not exist in the list of events. Returning empty inbox event.");
		return EmailEvents[0];
	}

	/// <summary>
	/// Return a random unlocked event of type T. No error checking.
	/// </summary>
	/// <returns>T event</returns>
	public T GetRandomEvent<T,U>() where T : IIsEvent<U>
	{
		if (typeof(T).Equals(typeof(EmailEvent)))
		{
			List<EmailEvent> unlocked_Events = new List<EmailEvent>();
			for (int i = 0; i < EmailEvents.Length; i++)
			{
				if (EmailEvents[i].IsUnlocked && !EmailEvents[i].IsCompleted)
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
			return EmailEvents[0];
		}
	
		return unlocked_Events[UnityEngine.Random.Range(0, unlocked_Events.Count)];
=======
>>>>>>> Stashed changes
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