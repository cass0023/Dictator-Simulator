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
	public string Text;
	[Tooltip("Which stats to change and by how much?")]
	public StatValPair[] StatsToChange;
}

[System.Serializable]
public struct StatValPair
{
	[Tooltip("Which stat is affected.")]
	public Stats lockType;
	[Range(-1, 1), Tooltip("How much is the stat affected. Negitive numbers for a decrease in the stat.")]
	public float StatVal;
}



[CreateAssetMenu(fileName = "New_Email", menuName = "ScriptableObjects/Email", order = 1)]
public class ScriptableEvent : ScriptableObject
{
	[Header("Event Identifier")]
	[Tooltip("A unique name for the event.")]
	public string EventName;
	
	[Header("Event Lock Options")]
	[Tooltip("How the event is unlocked. Leave the array empty if it is always unlocked.")]
	public UnlockEventByStatData[] StatLocks;
	[Tooltip("How the event is unlocked. Leave the array empty if it is always unlocked.")]
	public UnlockEventByWeekData[] WeekLocks;

	//TODO: Add event unlock

	[Header("Email Contents")]
	public string FromLine;
	public string ToLine;

	[TextArea]
	public string EmailContents;

	[Header("Responces")]
	[Tooltip("The responces the player can select in the email.")]
	public ResponceOption[] ResponceOptions;
}
