using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    public AudioSource mouseClickDown;
    public AudioSource mouseClickUp; 
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        interactText.SetActive(false);
        tvInteract = false;
        computerInteract = false;
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

        //checks trigger zones and if player is pressing interact
        if(Input.GetKeyDown(interact) && tvInteract)
        {
            InteractionManager.Instance.SwitchCamera("StatCamera");
        }
        if(Input.GetKeyDown(interact) && computerInteract){
            InteractionManager.Instance.SwitchCamera("ComCamera");

			//GameManager.Instance.LoadStaticEvents<EmailEvent, ScriptableEvent>();
			//GameManager.Instance.LoadStaticEvents<SocialEvent, ScriptableSocialMedia>();
			//GameManager.Instance.LoadStaticEvents<NewsEvent, ScriptableNews>();

		}
        if(Input.GetKeyDown(interact) && doorInteract){
            newWeekPopUp.SetActive(true);
        }
        if(Input.GetKeyDown(interact) && execOrderInteract){
            InteractionManager.Instance.SwitchCamera("ExecOrderCamera");
		}
        if(Input.GetKeyDown(interact) && calendarInteract){
            InteractionManager.Instance.SwitchCamera("CalendarCamera");
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			InteractionManager.Instance.SwitchCamera("PlayerCam");
			OrderManager.Instance.DisplayOrder();
		}
	}
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
            
            //disables player movement while at stations
            playerController.StopMouseMovement();
            playerController.canMove = false;
            
            //exit ui
            exitText.SetActive(true);
            interactText.SetActive(false);
            
            //computer audio clip
            if (Input.GetMouseButtonDown(0))
            {
                mouseClickDown.Play();
            }
        }
        //functional for now but mouse can move during new week transition
        //checks if door popup is active to stop movement
        else if (newWeekPopUp.activeSelf){
            playerController.StopMouseMovement();
            playerController.canMove = false;
        }
        //disables mouse movement for 2s while cameras transition
        else if(!newWeekPopUp.activeSelf){
            Invoke("AllowCameraTransition", 2f);
            exitText.SetActive(false);
        }
    }
    public void AllowCameraTransition(){
        //invokes so player cant move mouse until camera is fully transitioned
        playerController.AllowMouseMovement();
        playerController.canMove = true;
    }
    //checks what interact zone the player is triggering
    void OnTriggerEnter(Collider collider){
        if(canInteract == false){
            canInteract = true;
        }
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
    //resets all trigger interact zones after exit
    void OnTriggerExit(Collider collider){
        interactText.SetActive(false);
        canInteract = false;
		tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        calendarInteract = false;
        execOrderInteract = false;
        playerController.canMoveMouse = true;
	}
}
