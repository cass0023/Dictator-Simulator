using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New_Event", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class ScriptableEvent : ScriptableObject
{
	public string EventTitle;
	public string EventDescription;

	public Stats RequiredStat = Stats.NONE;
	public float RequiredAmount = 0;
	public float RequiredWeek = 0;

	public bool TriggeredPreviously = false;

	

	[Header("Choice 1")]
	public string Choice1Text;
	public Stats StatUp1 = Stats.NONE;
	public float AmountUp1;
	public Stats StatDown1 = Stats.NONE;
	public float AmountDown1;

	[Header("Choice 2")]
	public string Choice2Text;
	public Stats StatUp2 = Stats.NONE;
	public float AmountUp2;
	public Stats StatDown2 = Stats.NONE;
	public float AmountDown2;

	[Header("Choice 3")]
	public string Choice3Text;
	public Stats StatUp3 = Stats.NONE;
	public float AmountUp3;
	public Stats StatDown3 = Stats.NONE;
	public float AmountDown3;

	[Header("Choice 4")]
	public string Choice4Text;
	public Stats StatUp4 = Stats.NONE;
	public float AmountUp4;
	public Stats StatDown4 = Stats.NONE;
	public float AmountDown4;

}
