using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsManager
{
	private GameObject HeadlineTextObject;
	private GameObject BodyTextObject;
	private Sprite ImageTextObject;


	private static NewsManager instance = new NewsManager();

	NewsEvent CurrentEvent;

	private NewsManager()
	{

	}

	public static NewsManager Instance
	{
		get { return instance; }
	}

	public void InitializeNews(NewsEvent newsToLoad)
	{
		if (newsToLoad.Data.EventName == null)
		{
			Debug.LogError("Invalid event to intialize.");
			return;
		}	
		
		CurrentEvent = newsToLoad;

		HeadlineTextObject = GameObject.Find("T_NewsHeadline");
		HeadlineTextObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.NewsHeadline;
		BodyTextObject = GameObject.Find("T_NewsText");
		BodyTextObject.GetComponent<TextMeshProUGUI>().text = CurrentEvent.Data.NewsBody;

	}
}
