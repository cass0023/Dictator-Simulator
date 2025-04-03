using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager
{
    [SerializeField]
    GameObject GoodBackground;
	[SerializeField]
	GameObject NeutralBackground;
	[SerializeField]
	GameObject BadBackground;

    [SerializeField, Range(0f, 1f)]
    float WhenTurnNeutral = 0.25f;
    [SerializeField, Range(0f,1f)]
    float WhenTurnBad = 0.75f;

	bool Loaded = false;

	private static BackgroundManager instance = new BackgroundManager();

	private BackgroundManager()
	{

	}
	public static BackgroundManager Instance
	{
		get { return instance; }
	}
	public void CheckSwitchBackground(float fearAmount)
    {
		if (!Loaded)
		{
			GoodBackground = GameObject.Find("Good");
			NeutralBackground = GameObject.Find("Neutral");
			BadBackground = GameObject.Find("Bad");
			Loaded = true;
		}

		if (fearAmount < WhenTurnNeutral) 
        {
            //Good background
            GoodBackground.SetActive(true);
            NeutralBackground.SetActive(false);
            BadBackground.SetActive(false);
        }
        else if(fearAmount >= WhenTurnBad) 
        {
			GoodBackground.SetActive(false);
			NeutralBackground.SetActive(false);
			BadBackground.SetActive(true);
		}
        else
        {
			GoodBackground.SetActive(false);
			NeutralBackground.SetActive(true);
			BadBackground.SetActive(false);
		}
    }
	
}
