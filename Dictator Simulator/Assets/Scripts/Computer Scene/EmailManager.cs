using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailManager
{
	private EmailEvent CurEmailEvent;
	private List<Button> Buttons = new List<Button>();

	private static EmailManager instance = new EmailManager();
	private EmailManager()
	{

	}

	public static EmailManager Instance
	{
		get { return instance; }
	}

	public void InitializeEmail()
	{
		if(CurEmailEvent.Data.EventName == null)
		{
			Debug.LogError("Invalid event to intialize.");
			return;
		}

		GameObject.Find("T_FromLine").GetComponent<TextMeshProUGUI>().text = $"From: {CurEmailEvent.Data.FromLine}";
		GameObject.Find("T_ToLine").GetComponent<TextMeshProUGUI>().text = $"To: {CurEmailEvent.Data.ToLine}";
		GameObject.Find("T_EmailBody").GetComponent<TextMeshProUGUI>().text = $"{CurEmailEvent.Data.EmailContents}";

		//CurEmailEvent.Data.

		foreach(ResponceOption responce in CurEmailEvent.Data.ResponceOptions)
		{
			
		}

	}

	/// <summary>
	/// Set the current email to load. No error checking so it must be a valid EmailEvent.
	/// </summary>
	/// <param name="ee"></param>
	public void SetCurEmail(EmailEvent ee)
	{
		CurEmailEvent = ee;
	}

	//public static Button CreateButton(Button buttonPrefab, Canvas canvas)
	//{
	//	//var button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;
	//	//var rectTransform = button.GetComponent();
	//	//rectTransform.SetParent(canvas.transform);
	//	//rectTransform.anchorMax = cornerTopRight;
	//	//rectTransform.anchorMin = cornerBottomLeft;
	//	//rectTransform.offsetMax = Vector2.zero;
	//	//rectTransform.offsetMin = Vector2.zero;
	//	//return button;
	//}

}
