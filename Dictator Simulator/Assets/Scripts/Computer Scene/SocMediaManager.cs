using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SocMediaManager : MonoBehaviour
{
    //temp player stats for debugging
    public int debugSanity;
    public int debugApproval;
    public int debugTrust;
    public int debugFear;

    //inspector input list for tweet
    public List<SocMediaEvents> SocMediaEvents;
    
    //temp number for testing, checking if first input in events is working
    [SerializeField]private int x;
    
    //checks which button is currently pressed and what to display in tweet
    public GameObject[] buttonOptions;
    private bool hasSelected;
    private int selectedOption;
    private string tempSelectedOption;
    public TextMeshProUGUI tweetText;

    void Start(){
        debugApproval = 100;
        debugFear = 100;
        debugSanity = 100;
        debugTrust = 100;
    }
    void Update(){
        //changes tweet text depending on button pressed
        if(hasSelected){
            tweetText.text = SocMediaEvents[x].beforeBlank + " " + tempSelectedOption + " " + SocMediaEvents[x].afterBlank;
        }
        else{
            tweetText.text = SocMediaEvents[x].beforeBlank + " _________ " + SocMediaEvents[x].afterBlank;
        }
    }
    public void InitializeTweet(){
        tempSelectedOption = null;
        hasSelected = false;
        //randomly picks tweet for now
        x = Random.Range(0,SocMediaEvents.Count);
        //checks which button to set active based on num of options
        for(int i = 0; i < SocMediaEvents[x].Options.Count + 1; i++){
            buttonOptions[i].SetActive(true);
            for (int b = 0; b < SocMediaEvents[x].Options.Count - 1; b++){
                //changes button text to match options
                buttonOptions[i + 1].GetComponentInChildren<TextMeshProUGUI>().text = SocMediaEvents[x].Options[i].name;
            }
        }
    }
    //changes selected button to string to display in text
    public void OnOption1Click(){
        tempSelectedOption = SocMediaEvents[x].Options[0].name;
        selectedOption = 0;
        hasSelected = true;
    }
    public void OnOption2Click(){
        tempSelectedOption = SocMediaEvents[x].Options[1].name;
        selectedOption = 1;
        hasSelected = true;
    }
    public void OnOption3Click(){
        tempSelectedOption = SocMediaEvents[x].Options[2].name;
        selectedOption = 2;
        hasSelected = true;
    }
    public void OnOption4Click(){
        tempSelectedOption = SocMediaEvents[x].Options[3].name;
        selectedOption = 3;
        hasSelected = true;
    }
    public void OnOption5Click(){
        tempSelectedOption = SocMediaEvents[x].Options[4].name;
        selectedOption = 4;
        hasSelected = true;
    }
    public void OnOption6Click(){
        tempSelectedOption = SocMediaEvents[x].Options[5].name;
        selectedOption = 5;
        hasSelected = true;
    }
    public void OnPostClick(){
        if(hasSelected){
            //update stats based on list
            debugApproval += SocMediaEvents[x].Options[selectedOption].approval;
            debugFear += SocMediaEvents[x].Options[selectedOption].fear;
            debugSanity += SocMediaEvents[x].Options[selectedOption].sanity;
            debugTrust += SocMediaEvents[x].Options[selectedOption].trust;
            DisableOptionButtons();
        }
    }
    private void DisableOptionButtons(){
        for(int i = 0; i < buttonOptions.Length; i++){
            buttonOptions[i].SetActive(false);
        }
    }
}
