using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//List of all the stats that will have a value between 0 and 1. 
public enum Stats
{ 
    SANITY = 0,
    PERCEPTION = 1,
    STABILITY = 2


}


public class StatManager : MonoBehaviour
{
    //Dictionary that stores all of the stats and their float values. It is private, so call the public functions that change the stats individually.
    private Dictionary<Stats, float> StatValues = new Dictionary<Stats, float>();

    // Start is called before the first frame update
    void Start()
    {
        //Add new key value pairs
        StatValues.Add(Stats.SANITY, 1.0f) ;
		StatValues.Add(Stats.PERCEPTION, 1.0f);
		StatValues.Add(Stats.STABILITY, 1.0f);
	}

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
        }
    }
	public void DecreaseStat(Stats stat, float amount)
	{
		if (StatValues.ContainsKey(stat))
		{
			if (StatValues[stat] >= 0f) //Make sure not to go below 0% of a stat
			{
				StatValues[stat] = 0f;
			}
			else
			{
				StatValues[stat] += amount;
			}
		}
	}
}
