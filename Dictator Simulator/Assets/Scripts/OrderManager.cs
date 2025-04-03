using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OrderManager
{
	private GameObject OrderTitleObject;
	private GameObject OrderDetailsObject;

	private static OrderManager instance = new OrderManager();

	OrderEvent CurrentEvent;

	private event EventHandler<IncreaseStatEventArgs> IncreaseStat;

	private OrderManager()
	{
		IncreaseStat += StatManager.Instance.IncreaseStat;
	}

	public static OrderManager Instance
	{
		get { return instance; }
	}

	public void InitializeOrder(OrderEvent orderToLoad)
	{
		//was originally newsToLoad, changed for bug fixing
		if (orderToLoad.Data.EventName == null)
		{
			Debug.LogError("Invalid event to intialize.");
			return;
		}

		CurrentEvent = orderToLoad;
	}

	public void DisplayOrder()
	{
		OrderTitleObject = GameObject.Find("T_OrderTitle");
		OrderTitleObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.OrderTitle;
		OrderDetailsObject = GameObject.Find("T_OrderDetails");
		OrderDetailsObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.OrderDetails;

		if (CurrentEvent.Data.EventName != "Default_Order")
		{
			UnityAction act = new UnityAction(() => YesOnClick());
			GameObject.Find("Btn_YesEO").GetComponent<Button>().onClick.AddListener(act);

			UnityAction act2 = new UnityAction(() => NoOnClick());
			GameObject.Find("Btn_NoEO").GetComponent<Button>().onClick.AddListener(act2);
		}
	}

	private void YesOnClick()
	{
		foreach (StatValPair s in CurrentEvent.Data.StatChangeOnSign)
		{
			IncreaseStatEventArgs args = new()
			{
				StatToIncrease = s.EffectedStat,
				Amount = s.StatVal
			};
			IncreaseStat?.Invoke(CurrentEvent, args);
		}

		foreach (EventTypeNamePair e in CurrentEvent.Data.EventsToTriggerOnSign)
		{
			EventManager.Instance.UnlockEvent(e.TriggerEventName);
		}

		EventManager.Instance.CompleteEvent(CurrentEvent.Data.EventName);
		Debug.Log($"Signed Exec Order {CurrentEvent.Data.EventName}");
	}
	private void NoOnClick()
	{
		foreach (StatValPair s in CurrentEvent.Data.StatChangeOnDecline)
		{
			IncreaseStatEventArgs args = new()
			{
				StatToIncrease = s.EffectedStat,
				Amount = s.StatVal
			};
			IncreaseStat?.Invoke(CurrentEvent, args);
		}

		foreach (EventTypeNamePair e in CurrentEvent.Data.EventsToTriggerOnDecline)
		{
			EventManager.Instance.UnlockEvent(e.TriggerEventName);
		}
		EventManager.Instance.CompleteEvent(CurrentEvent.Data.EventName);
		Debug.Log($"Declined Exec Order {CurrentEvent.Data.EventName}");
	}

}
