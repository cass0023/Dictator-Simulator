using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public struct EmailEvent
{
	public ScriptableEvent Data;
	public bool IsUnlocked;
	public bool IsCompleted;
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
		}

		Loaded = true;
	}

	/// <summary>
	/// Call this to update events and check if they should be locked / unlocked.
	/// </summary>
	public void UpdateEventState()
	{
		if(!Loaded) { LoadEvents(); }

		if (EmailEvents != null)
		{
			for (int i = 0; i < EmailEvents.Length; i++) 
			{
				EmailEvents[i].IsUnlocked = CheckUnlock(EmailEvents[i]);
			}
		}
	}

	/// <summary>
	/// Checks if the current event is unlocked or not. Compares the current player stats to the specified value when the event was created.
	/// </summary>
	/// <param name="curEvent"></param>
	/// <returns></returns>
	private bool CheckUnlock(EmailEvent curEvent)
	{
		bool shouldUnlock = true;
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
		if(shouldUnlock) Debug.Log($"{curEvent.Data.name} is [UNLOCKED]");
		else Debug.Log($"{curEvent.Data.name} is [LOCKED]");

		return shouldUnlock;
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