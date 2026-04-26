using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour {

    public static PauseScreen Instance;
    

    void Awake()
    {
        Instance = this;
    }

    public void Setup(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (isShow)
        {
            AudioController.Instance.Pause();
        }
        
    }
    public void OnResume()
    {
        GameView.Instance.OnPause(false);
    }
    public void OnRestart()
    {
        GameView.Instance.Replay();
        gameObject.SetActive(false);
    }
    public void OnHome()
    {
        Home.Instance.Setup(true);
        gameObject.SetActive(false);
    }
}
