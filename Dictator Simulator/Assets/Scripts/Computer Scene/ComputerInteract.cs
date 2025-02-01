using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    //Computer Icon Interacts
    public GameObject[] computerPages;
    public GameObject[] buttons;
    public void OnEmailClick(){
        computerPages[0].SetActive(true);
        DeactivateButtons();
    }
    public void OnSocMediaClick(){
        computerPages[1].SetActive(true);
        DeactivateButtons();
    }
    public void OnNewsClick(){
        computerPages[2].SetActive(true);
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
}
