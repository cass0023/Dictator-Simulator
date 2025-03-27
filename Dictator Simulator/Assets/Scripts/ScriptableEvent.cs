using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Unity.Collections;
using UnityEngine;


public enum RequiredStatVal
{
	EQUAL = 0,
	ABOVE = 1,
	BELOW = 2
}

public enum EventType
{ 
	NONE = 0,
	EMAIL = 1,
	SOCIAL = 2,
	NEWS = 3,
	WINDOW = 4,
	EXEC_ORDER = 5
}

[System.Serializable]
public struct UnlockEventByStatData
{
	[Tooltip("Which stat is used to unlock the event. Select NONE if it starts off unlocked.")]
	public Stats lockType;
	[Range(0, 1), Tooltip("The required stat value to unlock this event.")]
	public float StatVal;
	[Tooltip("Determines if the stat must be above, below, or equal to the StatVal. Select NONE if the event is always unlocked.")]
	public RequiredStatVal RequiredStatVal;
}
[System.Serializable]
public struct UnlockEventByWeekData
{
	[Tooltip("The required stat value to unlock this event.")]
	public int WeekNum;
	[Tooltip("Determines if the event unlocks before or after the weeknum.")]
	public RequiredStatVal RequiredStatVal;
}

[System.Serializable]
public struct ResponceOption
{
	[Tooltip("What is displayed to the user.")]
	public string ResponceText;
	[Tooltip("Which stats to change and by how much?")]
	public StatValPair[] StatsToChange;

	[Tooltip("List of events to trigger when the user picks this responce.")]
	public EventTypeNamePair[] TriggerEventsList;

}

[System.Serializable]
public struct StatValPair
{
	[Tooltip("Which stat is affected.")]
	public Stats EffectedStat;
	[Range(-1, 1), Tooltip("How much is the stat affected. Negitive numbers for a decrease in the stat.")]
	public float StatVal;
}

[System.Serializable]
public struct EventTypeNamePair
{
	[Tooltip("The type of event to trigger.")]
	public EventType Type;
	[Tooltip("The EXACT name of the event you want to trigger.")]
	public string TriggerEventName;
}

public interface IUnlockable
{
	public string EventName { get;}
	public UnlockEventByStatData[] StatLocks { get; }
	public UnlockEventByWeekData[] WeekLocks { get;}
	public bool LockedByOtherEvent { get;}

}


/// <summary>
/// Event information for an email
/// </summary>
[CreateAssetMenu(fileName = "New_Email", menuName = "ScriptableObjects/Email", order = 2)]
public class ScriptableEvent : ScriptableObject, IUnlockable
{
	[Header("Event Identifier")]
	[Tooltip("A unique name for the event. Must match the file name!!!")]
	public string EventName;
	
	[Header("Event Lock Options")]
	[Tooltip("How the event is unlocked. Leave the array empty if it is always unlocked.")]
	public UnlockEventByStatData[] StatLocks;
	[Tooltip("How the event is unlocked. Leave the array empty if it is always unlocked.")]
	public UnlockEventByWeekData[] WeekLocks;
	[Tooltip("If the event is locked behind the player's responce in another event.")]
	public bool LockedByOtherEvent = false;
	//[HideInInspector] //Used for saving between play sessions. Needs to be implemented with a Completion variable
	//public bool HasBeenUnlockedByEvent = false;

	[Header("Email Contents")]
	public string FromLine;
	public string ToLine;

	[TextArea(1, 25)]
	public string EmailContents;

	[Header("Responces")]
	[Tooltip("The responces the player can select in the email.")]
	public ResponceOption[] ResponceOptions;

	string IUnlockable.EventName { get => EventName; }
	UnlockEventByStatData[] IUnlockable.StatLocks { get => StatLocks; }
	UnlockEventByWeekData[] IUnlockable.WeekLocks { get => WeekLocks;}
	bool IUnlockable.LockedByOtherEvent { get => LockedByOtherEvent; }
}
