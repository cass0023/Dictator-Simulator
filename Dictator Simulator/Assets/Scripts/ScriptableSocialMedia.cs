using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Unity.Collections;
using UnityEngine;

[Serializable]
public struct BlankInsert
{
	[Tooltip("The different options that the player can choose for each blank.")]
	public BlankOption[] Options;
	[TextArea(1, 3), Tooltip("The text after the blank text. Make sure to include leading spaces and a period at the end if needed.")]
	public string TextAfterBlank;
}

[Serializable]
public struct BlankOption
{
	[Tooltip("The name and text of the option for a specfic blank area.")]
	public string OptionName;
	[Tooltip("The stats that changes when this option is selected.")]
	public StatValPair[] StatsToChange;
}

[Serializable]
public struct SpecialCombo
{
	[Tooltip("The names of each option that when both are chosen, will do something to the stats in addition to their base stat change." +
		" For example, choosing two options that don't make sense together could decrease sanity.")]
	public string[] ComboNames;
	[Tooltip("The stats that changes when both options are selected.")]
	public StatValPair[] StatsToChange;
}

/// <summary>
/// Class for social media post scriptable objects. Note: This class only works with the structs from Scriptable event class in the same project.
/// </summary>
[CreateAssetMenu(fileName = "New_SocialPost", menuName = "ScriptableObjects/SocialPost", order = 3)]
public class ScriptableSocialMedia : ScriptableObject, IUnlockable
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

	[Header("Social Media Post Structure")]
	[TextArea (1,3), Tooltip("The text that appears before the first blank in the post.")]
	public string BeforeFirstBlank;

	[Tooltip("A list of each blank that appears in the post")]
	public BlankInsert[] BlankInserts;

	[Tooltip("If there are any special interactions between options chosen ")]
	public SpecialCombo[] SpecialCombos;

	string IUnlockable.EventName { get => EventName; }
	UnlockEventByStatData[] IUnlockable.StatLocks { get => StatLocks; }
	UnlockEventByWeekData[] IUnlockable.WeekLocks { get => WeekLocks; }
	bool IUnlockable.LockedByOtherEvent { get => LockedByOtherEvent; }

}
