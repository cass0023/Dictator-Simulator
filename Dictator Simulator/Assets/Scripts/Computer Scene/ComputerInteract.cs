using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ComputerInteract : MonoBehaviour
{
    //Computer Icon Interacts
    public GameObject[] computerPages;
    public GameObject[] buttons;
    private ScrollRect resetScroll;
    void Start(){

    }
    public void CloseComputer(){
        try{
            InteractionManager.Instance.SwitchCamera("PlayerCam");
            GetComponent<PlayerController>().AllowMouseMovement();
            GetComponent<PlayerController>().canMove = true;
            OrderManager.Instance.DisplayOrder();
        }
        catch { }
    }
    //checks which computer icon is pressed with button input
    public void OnEmailClick(){
        computerPages[0].SetActive(true);
        DeactivateButtons();
        EmailManager.Instance.DisplayEmail();
        resetScroll = GameObject.Find("EmailScrollView").GetComponent<ScrollRect>();
    }
    public void OnSocMediaClick(){
        computerPages[1].SetActive(true);
		SocMediaManager.Instance.DisplaySocial();
		DeactivateButtons();
    }
    public void OnNewsClick(){
        computerPages[2].SetActive(true);
        NewsManager.Instance.DisplayNews();
        DeactivateButtons();
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
        if(pageToDisable.name == "EmailPopUp"){
            resetScroll.verticalNormalizedPosition = 1f;
        }
		pageToDisable.SetActive(false);
        GameManager.Instance.LoadEvents();
        ReactivateButtons();
		
	}
}
