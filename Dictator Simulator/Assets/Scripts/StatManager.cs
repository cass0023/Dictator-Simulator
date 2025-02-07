using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

//List of all the stats that will have a value between 0 and 1. 
public enum Stats
{ 
	NONE = 0,
    SANITY = 1,
    APPROVAL = 2,
    TRUST= 3,
	FEAR = 4

}
/// <summary>
/// Class for handling stats in the game. There can only be one.
/// </summary>
public class StatManager
{
	//Instance of the class
    private static StatManager instance = new StatManager();
	//Dictionary that stores all of the stats and their float values. It is private, so call the public functions that change the stats individually.
	private readonly Dictionary<Stats, float> StatValues = new Dictionary<Stats, float>();
	//Store each string name of the UI element that the stat corrisponds to.
	private readonly Dictionary<Stats, string> UIStatName = new Dictionary<Stats, string>();
	//Store each slider name
	private readonly Dictionary<Stats, string> SliderNames = new Dictionary<Stats, string>();


	private StatManager() 
    {
		//Add new key value pairs
		StatValues.Add(Stats.SANITY, 1.0f);
        StatValues.Add(Stats.APPROVAL, 1.0f);
        StatValues.Add(Stats.TRUST, 1.0f);
		StatValues.Add(Stats.FEAR, 1.0f);

		UIStatName.Add(Stats.SANITY, "T_SanityVal");
		UIStatName.Add(Stats.APPROVAL, "T_ApprovalVal");
		UIStatName.Add(Stats.TRUST, "T_TrustVal");
		UIStatName.Add(Stats.FEAR, "T_FearVal");

		SliderNames.Add(Stats.SANITY, "SanitySlider");
		SliderNames.Add(Stats.APPROVAL, "ApprovalSlider");
		SliderNames.Add(Stats.TRUST, "TrustSlider");
		SliderNames.Add(Stats.FEAR, "FearSlider");
	}
	public static StatManager Instance
	{
		get { return instance; }
	}
	/// <summary>
	/// Call this method to increase the amount of a stat. Amount must be between 0 and 1. 
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="amount"></param>
    public void IncreaseStat(object sender, IncreaseStatEventArgs e)
    {
		if (StatValues.ContainsKey(e.StatToIncrease)) 
        {
            if (StatValues[e.StatToIncrease] >= 1.0f) //Make sure not to go above 100% of a stat
            {
				StatValues[e.StatToIncrease] = 1.0f;
			}
            else
            {
                 StatValues[e.StatToIncrease] += e.Amount;
            }
			UpdateText(e.StatToIncrease);
			UpdateSliders(e.StatToIncrease);

		}
    }
	/// <summary>
	/// Call this method to decrease the amount of a stat. Amount must be between 0 and 1. 
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="amount"></param>
	public void DecreaseStat(object sender, DecreaseStatEventArgs e)
	{
		if (StatValues.ContainsKey(e.StatToDecrease))
		{
			if (StatValues[e.StatToDecrease] <= 0f) //Make sure not to go below 0% of a stat
			{
				StatValues[e.StatToDecrease] = 0f;
			}
			else
			{
				StatValues[e.StatToDecrease] -= e.Amount;
			}
            UpdateText(e.StatToDecrease);
			UpdateSliders(e.StatToDecrease);
		}
	}
    /// <summary>
    /// Update the UI text to reflect the stat value.
    /// </summary>
    /// <param name="stat"></param>
    public void UpdateText(Stats stat)
    {
		try
		{
			int convertedStatVal = (int)Mathf.Round(StatValues[stat] * 100); //Make a percentage for displaying it.
			GameObject.Find(UIStatName[stat]).GetComponent<TextMeshProUGUI>().text = convertedStatVal.ToString() + "%";
		}
		catch (NullReferenceException) { }
    }
	/// <summary>
	/// Updates the sliders on the tv screen when a stat changes
	/// </summary>
	/// <param name="stat"></param>
	public void UpdateSliders(Stats stat)
	{
		try
		{
			GameObject.Find(SliderNames[stat]).GetComponent<UnityEngine.UI.Slider>().value = StatValues[stat];
		}
		catch (NullReferenceException) { }
	}

	public float GetStatValue(Stats stat)
	{
		return StatValues[stat];
	}

	

}
