using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SocMediaManager
{
	private GameObject ButtonsPanel;
	private GameObject SocialTextObject;

	private event EventHandler<IncreaseStatEventArgs> IncreaseStat;

	List<GameObject> Buttons = new List<GameObject>();

	private static SocMediaManager instance = new SocMediaManager();

	SocialEvent CurrentEvent;

	List<BlankOption> SelectedOptions = new List<BlankOption>();

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
		UnityAction act = new UnityAction(() => PostOnClick());
		GameObject.Find("ButtonPost").GetComponent<Button>().onClick.AddListener(act);

		CurrentEvent = socialToLoad;
		string socialText = $"{socialToLoad.Data.BeforeFirstBlank}";

		try
		{
			for (int i = 0; i < socialToLoad.Data.BlankInserts.Length; i++)
			{
				socialText += $" ______ {socialToLoad.Data.BlankInserts[i].TextAfterBlank}";

			}
		}
		catch(IndexOutOfRangeException) 
		{
			//Do nothing
		}

		SocialTextObject = GameObject.Find("T_SocialPost");
		SocialTextObject.GetComponent<TextMeshProUGUI>().text = socialText;

		ButtonsPanel = GameObject.Find("ButtonOptions");

		for (int i = 0; i < Buttons.Count; i++)
		{
			Canvas.Destroy(Buttons[i]); //Not working
		}
		Buttons.Clear();

		try
		{
			if (CurrentEvent.Data.BlankInserts[CurBlank].Options.Length > 0)
			{
				foreach (BlankOption option in CurrentEvent.Data.BlankInserts[CurBlank].Options)
				{
					CreateButton(option);
				}

			}
		}
		catch(IndexOutOfRangeException) { }
		
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
		if(SelectedOptions.Count < CurrentEvent.Data.BlankInserts.Length)
		{
			SelectedOptions.Add(option);
			UpdateSocialText();
			button.SetActive(false);
		}
	}

	/// <summary>
	/// When the post button is clicked
	/// </summary>
	public void PostOnClick()
	{
		if(CurrentEvent.Data.EventName == "Empty_Post")
		{
			return;
		}


		//Only do stuff if the blanks have been filled.
		if(SelectedOptions.Count == CurrentEvent.Data.BlankInserts.Length) 
		{
			foreach (BlankOption option in SelectedOptions)
			{
				foreach (StatValPair s in option.StatsToChange)
				{
					IncreaseStatEventArgs args = new()
					{
						StatToIncrease = s.EffectedStat,
						Amount = s.StatVal
					};
					IncreaseStat?.Invoke(option, args);
				}
			}

			
			foreach(SpecialCombo combo in CurrentEvent.Data.SpecialCombos)
			{	
				int comboPieces = 0;
				for(int i = 0; i < SelectedOptions.Count; i++) 
				{
					if (combo.ComboNames.Contains(SelectedOptions[i].OptionName))
					{
						comboPieces++;
					}
				}

				if(comboPieces >= combo.ComboNames.Length)
				{
					foreach(StatValPair s in combo.StatsToChange)
					{
						IncreaseStatEventArgs args = new()
						{
							StatToIncrease = s.EffectedStat,
							Amount = s.StatVal
						};
						IncreaseStat?.Invoke(combo, args);
					}
				}
				
			}

			EventManager.Instance.CompleteEvent(CurrentEvent.Data.EventName);
			Debug.Log($"Posted social media post: {SocialTextObject.GetComponent<TextMeshProUGUI>().text}");
			GameObject.Find("ComputerManager").GetComponent<ComputerInteract>().ClosePage(GameObject.Find("SocMediaPopUp"));
			
		}

	}

	void UpdateSocialText()
	{
		string currentText = CurrentEvent.Data.BeforeFirstBlank;

		for (int i = 0; i < CurrentEvent.Data.BlankInserts.Length; i++)
		{
			currentText += $" {SelectedOptions[i].OptionName} {CurrentEvent.Data.BlankInserts[i].TextAfterBlank}";
		}

		SocialTextObject.GetComponent<TextMeshProUGUI>().text = currentText;
	}
	
}
