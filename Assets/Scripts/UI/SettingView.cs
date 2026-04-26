using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingView : MonoBehaviour 
{
    public static SettingView Instance;

    void Awake(){
        Instance = this;
    }

    public void Setup(bool isShow)
    {
        gameObject.SetActive(!isShow);
    }
}
