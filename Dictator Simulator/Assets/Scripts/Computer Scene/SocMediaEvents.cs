using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]public class SocMediaEvents
{
    public string postName;
    public string beforeBlank;
    public string afterBlank;
    public List<StatSocMedia> Options; 
}
[Serializable]public class StatSocMedia{
    //gives name of option and how it effects the stats
    public string name;
    public int sanity;
    public int approval;
    public int trust;
    public int fear;
}
