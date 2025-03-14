using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager
{
	private GameObject OrderTitleObject;
	private GameObject OrderDetailsObject;

	private Button[] buttons;

	private static OrderManager instance = new OrderManager();

	OrderEvent CurrentEvent;

	private OrderManager()
	{

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

		OrderTitleObject = GameObject.Find("T_OrderTitle");
		OrderTitleObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.OrderTitle;
		OrderDetailsObject = GameObject.Find("T_OrderDetails");
		OrderDetailsObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.OrderDetails;

	}
}
