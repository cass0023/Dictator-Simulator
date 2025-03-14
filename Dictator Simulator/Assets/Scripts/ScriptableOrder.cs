using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Order", menuName = "ScriptableObjects/Order", order = 5)]
public class ScriptableOrder : ScriptableObject, IUnlockable
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

	[Header("Executive order info")]
	[TextArea(1, 3), Tooltip("The title of the order.")]
	public string OrderTitle;

	[TextArea(1, 10), Tooltip("Any details about what the order does.")]
	public string OrderDetails;

	string IUnlockable.EventName { get => EventName; }
	UnlockEventByStatData[] IUnlockable.StatLocks { get => StatLocks; }
	UnlockEventByWeekData[] IUnlockable.WeekLocks { get => WeekLocks; }
	bool IUnlockable.LockedByOtherEvent { get => LockedByOtherEvent; }
}
