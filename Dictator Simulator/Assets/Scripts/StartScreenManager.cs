using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene(1);
    }
}
