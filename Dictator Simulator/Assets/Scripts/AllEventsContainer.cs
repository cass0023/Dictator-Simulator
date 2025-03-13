using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Container class for all events
public class AllEventsContainer : MonoBehaviour
{
	[Header("Email Event List")]
	public ScriptableEvent[] Emails;

	[Header("Social Media Event List")]
	public ScriptableSocialMedia[] SocialMedia;

	[Header("News Event List")]
	public ScriptableNews[] News;

	[Header("Executive Order Event List")]
	public ScriptableEvent[] Orders;

	public static AllEventsContainer instance;

	private void Awake()
	{
		if(instance == null) 
		{
			instance = this; 
		}
		else
		{
			Destroy(this);
		}
		
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
}
