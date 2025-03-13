using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Cinemachine;

public class ComputerInteract : MonoBehaviour
{
    private SocMediaManager socMediaManager;
    //Computer Icon Interacts
    public GameObject[] computerPages;
    public GameObject[] buttons;
    private PlayerController playerController;
    void Start(){
        //socMediaManager = GetComponent<SocMediaManager>();
        playerController = GetComponent<PlayerController>();
    }
    public void CloseComputer(){
        try{
            InteractionManager.Instance.SwitchCamera("PlayerCam");
            GetComponent<PlayerController>().AllowMouseMovement();
            GetComponent<PlayerController>().canMove = true;
        }
        catch { }
    }
    //checks which computer icon is pressed with button input
    public void OnEmailClick(){
        computerPages[0].SetActive(true);
        DeactivateButtons();
		GameManager.Instance.LoadStaticEvents<EmailEvent, ScriptableEvent>(); //Setting stuff active / inactive causes problems searching for the UI to change
	}
    public void OnSocMediaClick(){
        computerPages[1].SetActive(true);
        DeactivateButtons();
        GameManager.Instance.LoadStaticEvents<SocialEvent, ScriptableSocialMedia>();
    }
    public void OnNewsClick(){
        computerPages[2].SetActive(true);
        DeactivateButtons();
		GameManager.Instance.LoadStaticEvents<NewsEvent, ScriptableNews>();
	}
    public void OnPrivateClick(){
        computerPages[3].SetActive(true);
        DeactivateButtons();
    }
    private void DeactivateButtons(){
        //deavtivates buttons while pages are open
        for(int i = 0; i < buttons.Length; i++){
            buttons[i].SetActive(false);
        }
    }
    public void ReactivateButtons(){
        //reavtivates buttons on ExitButton click
        for(int i = 0; i < buttons.Length; i++){
            buttons[i].SetActive(true);
        }
    }

    public void ClosePage(GameObject pageToDisable)
    {
		pageToDisable.SetActive(false);
        ReactivateButtons();
	}
}
