using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class EmailManager
{
	private GameObject ResponcePanel;
	
	private event EventHandler<IncreaseStatEventArgs> IncreaseStat;

	List<GameObject> Buttons = new List<GameObject>();

	private static EmailManager instance = new EmailManager();

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

		GameObject.Find("T_FromLine").GetComponent<TextMeshProUGUI>().text = $"From: {emailToLoad.Data.FromLine}";
		GameObject.Find("T_ToLine").GetComponent<TextMeshProUGUI>().text = $"To: {emailToLoad.Data.ToLine}";
		GameObject.Find("T_EmailBody").GetComponent<TextMeshProUGUI>().text = $"{emailToLoad.Data.EmailContents}";
		ResponcePanel = GameObject.Find("ResponcePanel");

		for (int i = 0; i < Buttons.Count; i++)
		{
			Canvas.Destroy(Buttons[i]); //Not working
		}
		Buttons.Clear();

		foreach(ResponceOption responce in emailToLoad.Data.ResponceOptions)
		{
			CreateButton(responce);
		}

		foreach(GameObject button in Buttons)
		{
			GameObject.Instantiate(button);
		}

	}

	private void CreateButton(ResponceOption responce)
	{
		GameObject rButton = new GameObject("Button");
		Button button = rButton.AddComponent<Button>();
		rButton.AddComponent<RectTransform>();
		rButton.AddComponent<CanvasRenderer>();
		rButton.AddComponent<Image>();

		GameObject tmpGameObject = new GameObject("Text (TMP)");
		TextMeshProUGUI txt = tmpGameObject.AddComponent<TextMeshProUGUI>();
		txt.text = responce.ResponceText;
		txt.color = Color.black;
		txt.autoSizeTextContainer = true;
		tmpGameObject.transform.SetParent(rButton.transform, false);
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
			IncreaseStat.Invoke(button, args);
		}

		foreach (EventTypeNamePair e  in responce.TriggerEventsList)
		{
			if(e.Type == EventType.EMAIL) 
			{
				EmailEvent ee = EventManager.Instance.GetEvent(e.TriggerEventName);
				ee.IsUnlocked = true;
			}
		}

	}

}
