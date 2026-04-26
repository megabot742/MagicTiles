using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MSong
{
    public string name, type;
    public int totalNote=30,tempo;
    public bool isUnlock;
    public AudioClip file;

    [HideInInspector]
    public int fav,id;
}
