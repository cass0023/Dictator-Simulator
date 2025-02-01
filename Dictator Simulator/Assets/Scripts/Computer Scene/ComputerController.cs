using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public KeyCode exitComputer;
    void Update(){
        if(Input.GetKeyDown(exitComputer)){
            GameManager.Instance.LoadScene("Office");
        }
    }
}
