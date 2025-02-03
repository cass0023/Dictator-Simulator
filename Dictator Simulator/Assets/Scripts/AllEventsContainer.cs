using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Container class for all events
public class AllEventsContainer : MonoBehaviour
{
	[Header("Email Event List")]
	public ScriptableEvent[] Emails;

	[Header("Social Media Event List")]
	public ScriptableEvent[] SocialMedia;

	[Header("Executive Order Event List")]
	public ScriptableEvent[] Orders;


	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
