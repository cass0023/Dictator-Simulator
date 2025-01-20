using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

//List of all the stats that will have a value between 0 and 1. 
public enum Stats
{ 
    SANITY = 0,
    PERCEPTION = 1,
    STABILITY = 2


}
/// <summary>
/// Class for handling stats in the game. There can only be one.
/// </summary>
public class StatManager
{
	//Instance of the class
    private static StatManager instance = new StatManager();
	//Dictionary that stores all of the stats and their float values. It is private, so call the public functions that change the stats individually.
	private Dictionary<Stats, float> StatValues = new Dictionary<Stats, float>();
	//Store each string name of the UI element that the stat corrisponds to.
	private Dictionary<Stats, string> UIStatName = new Dictionary<Stats, string>();

	private StatManager() 
    {
		//Add new key value pairs
		StatValues.Add(Stats.SANITY, 1.0f);
        StatValues.Add(Stats.PERCEPTION, 1.0f);
        StatValues.Add(Stats.STABILITY, 1.0f);

		UIStatName.Add(Stats.SANITY, "T_SanityVal");
		UIStatName.Add(Stats.PERCEPTION, "T_PerceptionVal");
		UIStatName.Add(Stats.STABILITY, "T_StabilityVal");

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
	
    public void IncreaseStat(Stats stat, float amount)
    {
        if(StatValues.ContainsKey(stat)) 
        {
            if (StatValues[stat] >= 1.0f) //Make sure not to go above 100% of a stat
            {
				StatValues[stat] = 1.0f;
			}
            else
            {
                 StatValues[stat] += amount;
            }
			UpdateText(stat);
		}
    }
	/// <summary>
	/// Call this method to decrease the amount of a stat. Amount must be between 0 and 1. 
	/// </summary>
	/// <param name="stat"></param>
	/// <param name="amount"></param>
	public void DecreaseStat(Stats stat, float amount)
	{
		if (StatValues.ContainsKey(stat))
		{
			if (StatValues[stat] <= 0f) //Make sure not to go below 0% of a stat
			{
				StatValues[stat] = 0f;
			}
			else
			{
				StatValues[stat] -= amount;
			}
            UpdateText(stat);
		}
	}
    /// <summary>
    /// Update the UI text to reflect the stat value.
    /// </summary>
    /// <param name="stat"></param>
    private void UpdateText(Stats stat)
    {
        int convertedStatVal = (int)Mathf.Round(StatValues[stat] * 100); //Make a percentage for displaying it.
		GameObject.Find(UIStatName[stat]).GetComponent<TextMeshProUGUI>().text = convertedStatVal.ToString() + "%";
    }

	

}
