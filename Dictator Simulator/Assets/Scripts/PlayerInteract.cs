using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //interact variables
    private bool tvInteract, computerInteract, doorInteract;
    [SerializeField]private KeyCode interact;
    public GameObject newWeekPopUp;
    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        tvInteract = false;
        computerInteract = false;
        doorInteract = false;
    }
    void Update()
    {
        CheckInput();
    }
        //Triggers and Interact
    public void CheckInput(){
        if(Input.GetKeyDown(interact) && tvInteract)
        {
            InteractionManager.Instance.SwitchCamera("StatCamera");
            playerController.canMove = false;
        }
		if (Input.GetKeyDown(KeyCode.Escape) && tvInteract || Input.GetKeyDown(KeyCode.Escape) && computerInteract)
		{
			InteractionManager.Instance.SwitchCamera("PlayerCam");
            playerController.AllowMouseMovement();
            playerController.canMove = true;
		}
        if(Input.GetKeyDown(interact) && computerInteract){
            InteractionManager.Instance.SwitchCamera("ComCamera");
            //playerController.canMove = false;
            playerController.StopMouseMovement();
        }
        if(Input.GetKeyDown(interact) && doorInteract){
            playerController.canMoveMouse = false;
            Debug.Log("Door interacted");
            newWeekPopUp.SetActive(true);
        }
	}
        void OnTriggerEnter(Collider collider){
        if (collider.gameObject.name == "StatScreenZone"){
            tvInteract = true;
        }
        if(collider.gameObject.name == "ComputerScreenZone"){
            computerInteract = true;
        }
        if(collider.gameObject.name == "DoorInteractZone"){
            doorInteract = true;
        }
    }
    void OnTriggerExit(Collider collider){
		tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        playerController.canMoveMouse = true;
	}
}
