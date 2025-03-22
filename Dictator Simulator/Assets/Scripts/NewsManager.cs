using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsManager : MonoBehaviour
{
    public List<string> newsHeadline;
    private GameObject headlineText;
    void Update()
    {
        try{
            GameObject.Find("T_NewsHeadline").GetComponent<TextMeshProUGUI>().text = newsHeadline[GameManager.Instance.WeekNum];
        }
        catch{

        }
    }
}
