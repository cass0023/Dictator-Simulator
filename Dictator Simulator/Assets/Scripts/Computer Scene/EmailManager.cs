using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class EmailManager
{
	private GameObject ResponcePanel;
	
	private event EventHandler<IncreaseStatEventArgs> IncreaseStat;

	List<GameObject> Buttons = new List<GameObject>();

	private static EmailManager instance = new EmailManager();

	EmailEvent CurrentEvent;

	private EmailManager()
	{
		IncreaseStat += StatManager.Instance.IncreaseStat;
	}

	public static EmailManager Instance
	{
		get { return instance; }
	}

	/// <summary>
	/// Initialize a specific email from its file name. Does not check if it is unlocked or not.
	/// </summary>
	/// <param name="emailToLoad"></param>
	public void InitializeEmail(EmailEvent emailToLoad)
	{
		if(emailToLoad.Data.EventName == null)
		{
			Debug.LogError("Invalid event to intialize.");
			return;
		}
		CurrentEvent = emailToLoad;

		GameObject.Find("T_FromLine").GetComponent<TextMeshProUGUI>().text = $"From: {emailToLoad.Data.FromLine}";
		GameObject.Find("T_ToLine").GetComponent<TextMeshProUGUI>().text = $"To: {emailToLoad.Data.ToLine}";
		GameObject.Find("T_EmailBody").GetComponent<TextMeshProUGUI>().text = $"{emailToLoad.Data.EmailContents}";
		ResponcePanel = GameObject.Find("ResponcePanel");

		for (int i = 0; i < Buttons.Count; i++)
		{
			Canvas.Destroy(Buttons[i]); //Not working
		}
		Buttons.Clear();

		foreach(ResponceOption responce in CurrentEvent.Data.ResponceOptions)
		{
			CreateButton(responce);
		}

		Canvas.ForceUpdateCanvases();
	}

	private void CreateButton(ResponceOption responce)
	{
		GameObject rButton = new GameObject("Btn_" + responce.ResponceText);
		Button button = rButton.AddComponent<Button>();
		rButton.AddComponent<RectTransform>();
		rButton.AddComponent<CanvasRenderer>();
		rButton.AddComponent<Image>();
		rButton.AddComponent<HorizontalLayoutGroup>();
		button.targetGraphic = rButton.GetComponent<Image>();
		

		GameObject tmpGameObject = new GameObject("Text (TMP)");
		TextMeshProUGUI txt = tmpGameObject.AddComponent<TextMeshProUGUI>();
		txt.text = responce.ResponceText;
		txt.color = Color.black;
		
		tmpGameObject.transform.SetParent(rButton.transform, false);
		txt.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
		txt.rectTransform.anchorMax = Vector2.one * 0.9f;
		txt.rectTransform.anchoredPosition = Vector3.zero;
		txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
		txt.verticalAlignment = VerticalAlignmentOptions.Middle;
		txt.autoSizeTextContainer = true;
		UnityAction act = new UnityAction(() => ResponceOnClick(rButton, responce));
		button.onClick.AddListener(act);

		rButton.transform.SetParent(ResponcePanel.transform, false);
		
		Buttons.Add(rButton);
	}

	void ResponceOnClick(GameObject button, ResponceOption responce)
	{
		foreach (StatValPair s in responce.StatsToChange)
		{
			IncreaseStatEventArgs args = new()
			{
				StatToIncrease = s.EffectedStat,
				Amount = s.StatVal
			};
			IncreaseStat?.Invoke(button, args);
		}

		foreach (EventTypeNamePair e in responce.TriggerEventsList)
		{
			if (e.Type == EventType.EMAIL)
			{
				EmailEvent ee = EventManager.Instance.GetEvent<EmailEvent, ScriptableEvent>(e.TriggerEventName);
				ee.HasBeenUnlockedByEvent = true;
				Debug.Log($"{ee.Data.EventName} Has been unlocked by another event");
			}
			else if (e.Type == EventType.SOCIAL) 
			{
				SocialEvent ee = EventManager.Instance.GetEvent<SocialEvent, ScriptableSocialMedia>(e.TriggerEventName);
				ee.HasBeenUnlockedByEvent = true;
				Debug.Log($"{ee.Data.EventName} Has been unlocked by another event");
			}
		}


		EventManager.Instance.CompleteEvent(CurrentEvent.Data.EventName);
		GameObject.Find("ComputerManager").GetComponent<ComputerInteract>().ClosePage(GameObject.Find("EmailPopUp"));
		Debug.Log($"Clicked button {button.name}.");
		
	}

}
