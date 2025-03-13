using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New_News", menuName = "ScriptableObjects/News", order = 4)]
public class ScriptableNews : ScriptableObject, IUnlockable
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

	[Header("News Content")]
	[TextArea(1, 3), Tooltip("The Headline of the news.")]
	public string NewsHeadline;

	[TextArea(1, 10), Tooltip("The content of the news.")]
	public string NewsBody;

	[Tooltip("Image of the news.")]
	public Sprite NewsImage;

	string IUnlockable.EventName { get => EventName; }
	UnlockEventByStatData[] IUnlockable.StatLocks { get => StatLocks; }
	UnlockEventByWeekData[] IUnlockable.WeekLocks { get => WeekLocks; }
	bool IUnlockable.LockedByOtherEvent { get => LockedByOtherEvent; }


}
