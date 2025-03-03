using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //interact variables
    private bool tvInteract, computerInteract, doorInteract, execOrderInteract, calendarInteract;
    [SerializeField]private KeyCode interact;
    public GameObject newWeekPopUp;
    public GameObject debugMenu;
    private PlayerController playerController;
    public GameObject interactText;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        tvInteract = false;
        computerInteract = false;
        execOrderInteract = false;
        calendarInteract = false;
        doorInteract = false;
    }
    void Update()
    {
        CheckInput();
    }
    //Triggers and Interact
    public void CheckInput(){
        //shows debug menu
        if (Input.GetKeyDown(KeyCode.BackQuote)){
            debugMenu.SetActive(!debugMenu.activeSelf);
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
        if(Input.GetKeyDown(interact) && execOrderInteract){
            InteractionManager.Instance.SwitchCamera("ExecOrderCamera");
        }
        if(Input.GetKeyDown(interact) && tvInteract)
        {
            InteractionManager.Instance.SwitchCamera("StatCamera");
            playerController.canMove = false;
        }
        if (Input.GetKeyDown(interact) && calendarInteract){
            InteractionManager.Instance.SwitchCamera("CalendarCamera");
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            // && tvInteract || Input.GetKeyDown(KeyCode.Escape) && computerInteract
			InteractionManager.Instance.SwitchCamera("PlayerCam");
            playerController.AllowMouseMovement();
            playerController.canMove = true;
		}
	}
        void OnTriggerEnter(Collider collider){
        interactText.SetActive(true);
        if (collider.gameObject.name == "StatScreenZone"){
            tvInteract = true;
        }
        if(collider.gameObject.name == "ComputerScreenZone"){
            computerInteract = true;
        }
        if(collider.gameObject.name == "DoorInteractZone"){
            doorInteract = true;
        }
        if(collider.gameObject.name == "EOInteractZone"){
            execOrderInteract = true;
        }
        if(collider.gameObject.name == "CalendarInteractZone"){
            calendarInteract = true;
        }
    }
    void OnTriggerExit(Collider collider){
        interactText.SetActive(false);
		tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        execOrderInteract = false;
        calendarInteract = false;
        playerController.canMoveMouse = true;
	}
}
