using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public struct EventChoice
{
	public int ChoiceIndex;

	public string ChoiceText;
	public string UIElementName;

	public Dictionary<Stats, float> AmountToChangeEachStat;
}

public class EventManager
{

	private static EventManager instance = new EventManager();
	private EventManager()
	{
		
	}

	public static EventManager Instance
	{
		get { return instance; }
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

public class StatReachedEventArgs : EventArgs
{
	public Stats RequiredStat;
	public float RequiredAmount;
	public bool TriggeredPreviously = false;

	public string EventTitle;
	public string EventDescription;

	public List<EventChoice> EventChoices;
}

public class WeekReachedEventArgs : EventArgs
{
	public bool TriggeredPreviously = false;

	public string EventTitle;
	public string EventDescription;

	public List<EventChoice> EventChoices;
}

