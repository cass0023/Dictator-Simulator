using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionManager
{
	private static InteractionManager instance = new InteractionManager();

	CinemachineVirtualCamera[] AllCameras;

	private InteractionManager()
	{
		AllCameras = GameObject.FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);
	}

	public static InteractionManager Instance
	{
		get { return instance; }
	}

	/// <summary>
	/// Switch the camera from current camera to the cam name provided. Defaults to player camera if invalid. Must match the camera name in hierarchy.
	/// </summary>
	/// <param name="CamName"></param>
	public void SwitchCamera(string CamName)
	{
		bool setCam = false;
		foreach (CinemachineVirtualCamera cam in AllCameras) 
		{
			cam.Priority = 1;

			if(cam.name == CamName) 
			{
				cam.Priority = 10;
				setCam = true;

			}
		}

		if (!setCam) 
		{
			//If the camera name is invalid, set it to be the player name
			GameObject.Find("Player").GetComponent<CinemachineVirtualCamera>().Priority = 10;
			Debug.Log($"Invalid camera to switch to {CamName}. Defaulted back to player camera.");
		}
		
	}


}
