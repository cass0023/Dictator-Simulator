using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    //interact variables
    private bool tvInteract, computerInteract, doorInteract, execOrderInteract, calendarInteract;
    [SerializeField]private KeyCode interact;
    private PlayerController playerController;
    //UI
    private bool canInteract;
    public GameObject interactText;
    public GameObject exitText;
    public GameObject newWeekPopUp;
    public GameObject debugMenu;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        canInteract = false;
        tvInteract = false;
        computerInteract = false;
        execOrderInteract = false;
        calendarInteract = false;
        doorInteract = false;
    }
    void Update()
    {
        CheckInput();
        InteractUI();
    }
    //Triggers and Interact
    public void CheckInput(){
        //shows debug menu
        if (Input.GetKeyDown(KeyCode.BackQuote)){
            debugMenu.SetActive(!debugMenu.activeSelf);
        }
        if(Input.GetKeyDown(interact) && computerInteract){
            InteractionManager.Instance.SwitchCamera("ComCamera");
            playerController.canMove = false;
            playerController.StopMouseMovement();
        }
        if(Input.GetKeyDown(interact) && doorInteract){
            playerController.canMoveMouse = false;
            newWeekPopUp.SetActive(true);
        }
        if(Input.GetKeyDown(interact) && execOrderInteract){
            InteractionManager.Instance.SwitchCamera("ExecOrderCamera");
            playerController.canMove = false;
        }
        if(Input.GetKeyDown(interact) && tvInteract)
        {
            InteractionManager.Instance.SwitchCamera("StatCamera");
            playerController.canMove = false;
        }
        if (Input.GetKeyDown(interact) && calendarInteract){
            InteractionManager.Instance.SwitchCamera("CalendarCamera");
            playerController.canMove = false;
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			InteractionManager.Instance.SwitchCamera("PlayerCam");
            playerController.AllowMouseMovement();
            playerController.canMove = true;
		}
	}
    //turns interaction ui on and off 
    void InteractUI(){
        //interact ui
        //changes based on if player enters trigger or presses interact
        if(canInteract){
            interactText.SetActive(true);
        }
        if(canInteract = false || Input.GetKeyDown(interact) && interactText.activeSelf){
            canInteract = false;
            interactText.SetActive(false);
        }
        //exit ui
        //turns off and on based on what camera is active
        CinemachineBrain ActiveCamera = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        if(ActiveCamera.ActiveVirtualCamera.Name != "PlayerCam"){
            exitText.SetActive(true);
        }
        else{
            exitText.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider collider){
        if(canInteract == false){
            canInteract = true;
        }
        //triggers interactable objects in checkinput()
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
        canInteract = false;
		tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        execOrderInteract = false;
        calendarInteract = false;
        playerController.canMoveMouse = true;
	}
}
