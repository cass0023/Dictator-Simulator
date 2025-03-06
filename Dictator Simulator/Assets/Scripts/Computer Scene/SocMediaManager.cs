using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SocMediaManager
{
	//temp player stats for debugging
	private GameObject ButtonsPanel;

	private event EventHandler<IncreaseStatEventArgs> IncreaseStat;

	List<GameObject> Buttons = new List<GameObject>();

	private static SocMediaManager instance = new SocMediaManager();

	SocialEvent CurrentEvent;

	private SocMediaManager()
	{
		IncreaseStat += StatManager.Instance.IncreaseStat;
	}

	public static SocMediaManager Instance
	{
		get { return instance; }
	}

	//temp number for testing, checking if first input in events is working
	[SerializeField]private int CurBlank = 0;

	/// <summary>
	/// Initialize a specific email from its file name. Does not check if it is unlocked or not.
	/// </summary>
	/// <param name="socialToLoad"></param>
	public void InitializeSocial(SocialEvent socialToLoad)
	{
		if (socialToLoad.Data.EventName == null)
		{
			Debug.LogError("Invalid event to intialize.");
			return;
		}
		CurrentEvent = socialToLoad;
		string socialText = $"{socialToLoad.Data.BeforeFirstBlank}";
		
		for (int i = 0; i < socialToLoad.Data.BlankInserts.Length; i++) 
		{
			socialText += $" ______ {socialToLoad.Data.BlankInserts[i].TextAfterBlank}";
			
		}
		
		GameObject.Find("T_SocialPost").GetComponent<TextMeshProUGUI>().text = socialText;




		//GameObject.Find("T_ToLine").GetComponent<TextMeshProUGUI>().text = $"To: {emailToLoad.Data.ToLine}";
		//GameObject.Find("T_EmailBody").GetComponent<TextMeshProUGUI>().text = $"{emailToLoad.Data.EmailContents}";
		ButtonsPanel = GameObject.Find("ButtonOptions");

		for (int i = 0; i < Buttons.Count; i++)
		{
			Canvas.Destroy(Buttons[i]); //Not working
		}
		Buttons.Clear();

		if (CurrentEvent.Data.BlankInserts[CurBlank].Options.Length > 0)
		{
			foreach (BlankOption option in CurrentEvent.Data.BlankInserts[CurBlank].Options)
			{
				CreateButton(option);
			}

		}
		
		Canvas.ForceUpdateCanvases();
	}

	private void CreateButton(BlankOption option)
	{
		GameObject oButton = new GameObject("Btn_" + option.OptionName);
		Button button = oButton.AddComponent<Button>();
		oButton.AddComponent<RectTransform>();
		oButton.AddComponent<CanvasRenderer>();
		oButton.AddComponent<Image>();
		button.targetGraphic = oButton.GetComponent<Image>();


		GameObject tmpGameObject = new GameObject("Text (TMP)");
		TextMeshProUGUI txt = tmpGameObject.AddComponent<TextMeshProUGUI>();
		txt.text = option.OptionName;
		txt.color = Color.black;

		tmpGameObject.transform.SetParent(oButton.transform, false);
		txt.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
		txt.rectTransform.anchorMax = Vector2.one * 0.9f;
		txt.rectTransform.anchoredPosition = Vector3.zero;
		txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
		txt.verticalAlignment = VerticalAlignmentOptions.Middle;
		txt.autoSizeTextContainer = true;
		UnityAction act = new UnityAction(() => ResponceOnClick(oButton, option));
		button.onClick.AddListener(act);

		oButton.transform.SetParent(ButtonsPanel.transform, false);

		Buttons.Add(oButton);
	}

	void ResponceOnClick(GameObject button, BlankOption option)
	{
		//foreach (StatValPair s in responce.StatsToChange)
		//{
		//	IncreaseStatEventArgs args = new()
		//	{
		//		StatToIncrease = s.EffectedStat,
		//		Amount = s.StatVal
		//	};
		//	IncreaseStat?.Invoke(button, args);
		//}

		//foreach (EventTypeNamePair e in responce.TriggerEventsList)
		//{
		//	if (e.Type == EventType.EMAIL)
		//	{
		//		EmailEvent ee = EventManager.Instance.GetEvent(e.TriggerEventName);
		//		ee.HasBeenUnlockedByEvent = true;
		//		Debug.Log($"{ee.Data.EventName} Has been unlocked by another event");
		//	}
		//}


		//EventManager.Instance.CompleteEvent(CurrentEvent.Data.EventName);
		//GameObject.Find("ComputerManager").GetComponent<ComputerInteract>().ClosePage(GameObject.Find("EmailPopUp"));
		//Debug.Log($"Clicked button {button.name}.");

	}
	
}
