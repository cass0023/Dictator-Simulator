using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public interface IIsEvent
{
	bool HasBeenUnlockedByEvent { get; set; }
	bool IsUnlocked { get; set; }
	bool IsCompleted { get; set; }
	IUnlockable Data { get; }
}

public interface IIsEvent<T> : IIsEvent where T : IUnlockable
{
	new T Data { get; set; }
}

public class EmailEvent : IIsEvent<ScriptableEvent>
{
	public ScriptableEvent Data { get; set; }
	IUnlockable IIsEvent.Data => Data; // Explicit implementation for IIsEvent
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}
public class SocialEvent : IIsEvent<ScriptableSocialMedia>
{
	public ScriptableSocialMedia Data { get; set; }
	IUnlockable IIsEvent.Data => Data; // Explicit implementation for IIsEvent
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}
public class NewsEvent : IIsEvent<ScriptableNews>
{
	public ScriptableNews Data { get; set; }
	IUnlockable IIsEvent.Data => Data; // Explicit implementation for IIsEvent
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}
public class OrderEvent : IIsEvent<ScriptableOrder>
{
	public ScriptableOrder Data { get; set; }
	IUnlockable IIsEvent.Data => Data; // Explicit implementation for IIsEvent
	public bool HasBeenUnlockedByEvent { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsCompleted { get; set; }
}

public class EventManager
{

	private AllEventsContainer AllEvents;
	bool Loaded = false;

	private GameObject emailNotif;
	private GameObject socMediaNotif;

	private static EventManager instance = new EventManager();

	List<IIsEvent> EventList;

	private EventManager()
	{

	}

	public static EventManager Instance
	{
		get { return instance; }
	}

	public void LoadInitialEvents()
	{
		EventList = new List<IIsEvent>();
		AllEvents = GameObject.Find("AllEventsContainer").GetComponent<AllEventsContainer>();

		//Load Emails
		for (int i = 0; i < AllEvents.Emails.Length; i++)
		{
			EmailEvent emailEvent = new() 
			{ 
				Data = AllEvents.Emails[i],
				HasBeenUnlockedByEvent = false,
				IsUnlocked = false,
				IsCompleted = false
			};
			EventList.Add(emailEvent);
		}
		//Load social media posts
		for (int i = 0; i < AllEvents.SocialMedia.Length; i++)
		{
			SocialEvent socialEvent = new()
			{
				Data = AllEvents.SocialMedia[i],
				HasBeenUnlockedByEvent = false,
				IsUnlocked = false,
				IsCompleted = false
			};
			EventList.Add(socialEvent);
		}

		//Load News
		for (int i = 0; i < AllEvents.News.Length; i++)
		{
			NewsEvent newsEvent = new()
			{
				Data = AllEvents.News[i],
				HasBeenUnlockedByEvent = false,
				IsUnlocked = false,
				IsCompleted = false
			};
			EventList.Add(newsEvent);
		}

		for (int i = 0; i < AllEvents.Orders.Length; i++)
		{
			OrderEvent orderEvent = new()
			{
				Data = AllEvents.Orders[i],
				HasBeenUnlockedByEvent = false,
				IsUnlocked = false,
				IsCompleted = false
			};
			EventList.Add(orderEvent);
		}

		emailNotif = GameObject.Find("EmailNotif");
		socMediaNotif = GameObject.Find("SocMediaNotif");

		Loaded = true;
		Debug.Log("Loaded all events.");
	}

	/// <summary>
	/// Call this to update events and check if they should be locked / unlocked.
	/// </summary>
	public void UpdateEventState()
	{
		if (EventList.Count == 0)
		{
			Debug.LogWarning("EventList is empty. No events to update.");
			return;
		}
		foreach (var eventItem in EventList)
		{
			if (eventItem == null)
			{
				Debug.LogError("Null event found in EventList. Skipping...");
				continue;
			}

			eventItem.IsUnlocked = CheckUnlock(eventItem);
		}
	}

	/// <summary>
	/// Checks if the current event is unlocked or not. Compares the current player stats to the specified value when the event was created.
	/// Also checks to see if an event is unlocked by another event.
	/// </summary>
	/// <param name="curEvent"></param>
	/// <returns></returns>
	private bool CheckUnlock(IIsEvent curEvent)
	{
		if (curEvent.IsCompleted) return false;
		if (curEvent.IsUnlocked) return true;


		bool shouldUnlock = true;

		if (curEvent.Data.LockedByOtherEvent) //Get the property with the name LockedByOtherEvent in Data class
		{
			shouldUnlock &= curEvent.HasBeenUnlockedByEvent;
		}

		if (curEvent.Data.StatLocks.Length > 0)
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

		if (shouldUnlock)
		{
			Debug.Log($"{curEvent.Data.EventName} is [UNLOCKED]");
		}
		
		return shouldUnlock;
	}
	/// <summary>
	/// Get the event with a certain name, regardless if it is unlocked or not. If the name does not exits, return null. Does not care about the type of event.
	/// </summary>
	/// <param name="EventName"></param>
	/// <returns></returns>
	public IIsEvent GetEvent(string eventName)
	{
		if (EventList == null || EventList.Count == 0)
		{
			Debug.LogError("EventList is empty! Cannot find event.");
			return null;
		}

		var foundEvent = EventList.FirstOrDefault(e => e.Data.EventName == eventName);

		if (foundEvent == null)
		{
			Debug.LogError($"No event with name '{eventName}' exists in the list.");
			return null;
		}

		return foundEvent;
	}

	/// <summary>
	/// Return next unlocked event of type T.
	/// </summary>
	/// <returns></returns>
	public T GetNextEvent<T, U>() where T : class, IIsEvent<U> where U : IUnlockable
	{
		T firstMatch = null;
		
		foreach (var eventItem in EventList) 
		{
			if (eventItem is T typedEvent)
			{
				if (typedEvent.IsUnlocked && !typedEvent.IsCompleted)
				{
					if(firstMatch != null) return typedEvent; // Return the first unlocked, not completed, event of type T after the first occurrence.

					firstMatch = typedEvent; // Store the first occurrence for fallback.

				}
			}
		}
		if (firstMatch != null)
		{
			Debug.Log($"No unlocked events found. Returning {firstMatch.Data.EventName}");
			return firstMatch;
		}

		throw new InvalidOperationException($"No events of type {typeof(T).Name} exist in the list.");

	}

	public void CompleteEvent(string eventName)
	{
		IIsEvent firstOfType = null;
		foreach (var eventItem in EventList)
		{
			if (firstOfType == null || eventItem.GetType() == firstOfType.GetType())
			{
				firstOfType ??= eventItem; // Assign first occurrence of this type
				if (eventItem.Data.EventName == eventName && eventItem != firstOfType && !eventItem.IsCompleted)
				{
					eventItem.IsCompleted = true;
					Debug.Log($"Set {eventItem.Data.EventName} to completed.");
					return; // Stop after completing the first match
				}
			}
		}

		Debug.LogWarning($"Event with name '{eventName}' not found or cannot be completed.");
	}
	public void UnlockEvent(string eventName = null)
	{
		if (EventList == null || EventList.Count == 0)
		{
			Debug.LogError("EventList is empty! Cannot unlock event.");
			return;
		}

		// Find the first event of type T that matches the given name (if provided)
		var eventToUnlock = EventList
			.FirstOrDefault(e => eventName == null || e.Data.EventName == eventName);

		if (eventToUnlock == null)
		{
			Debug.LogError($"No event with name '{eventName}' found.");
			return;
		}

		if (!eventToUnlock.HasBeenUnlockedByEvent)
		{
			eventToUnlock.HasBeenUnlockedByEvent = true;
		}
		else
		{
			Debug.LogError($"Event {eventToUnlock.Data.EventName} has already been unlocked!");
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