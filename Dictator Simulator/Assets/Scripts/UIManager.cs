using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Create an EventHandler for each event
	public event EventHandler<IncreaseStatEventArgs> IncreaseStat;
	public event EventHandler<DecreaseStatEventArgs> DecreaseStat;

	// Start is called before the first frame update
	void Start()
    {
		//Register event listeners
		IncreaseStat += StatManager.Instance.IncreaseStat;
		DecreaseStat += StatManager.Instance.DecreaseStat;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	/// <summary>
	/// Function called when the increase button is clicked
	/// </summary>
	/// <param name="stats"></param>
    public void OnIncreaseClick(int stats)
    {
        IncreaseStatEventArgs args = new()
        {
            StatToIncrease = (Stats)stats,
            Amount = 0.1f
        };
		
		OnIncreaseStat(args);
	}
	/// <summary>
	/// Function called when the decrease button is clicked
	/// </summary>
	/// <param name="stats"></param>
	public void OnDecreaseClick(int stats)
	{
		DecreaseStatEventArgs args = new()
		{
			StatToDecrease = (Stats)stats,
			Amount = 0.1f
		};

		OnDecreaseStat(args);
	}

	
	//Methods for invoking events

	/// <summary>
	/// Send the increase stat event arguments to all listening objects
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnIncreaseStat(IncreaseStatEventArgs e)
	{
		IncreaseStat?.Invoke(gameObject, e);
	}
	protected virtual void OnDecreaseStat(DecreaseStatEventArgs e)
	{
		DecreaseStat?.Invoke(gameObject, e);
	}


}
