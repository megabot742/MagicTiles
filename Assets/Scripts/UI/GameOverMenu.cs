using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu Instance;

    public TMP_Text txtScore;

    int bestScore, score;
   

    RectTransform rect;
    MSong activeSong;

    #region Unity Scripts
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rect = GetComponent<RectTransform>();
        
       
    }
    #endregion
    public void Setup(bool isShow)
    {
        if (!isShow)
        {
            gameObject.SetActive(false);
        }
        else
        {

            Show();
            
        }
    }
    void Show()
    {
        AudioController.Instance.Play(AudioController.SOUND_GAME_OVER);
        gameObject.SetActive(true);
       
        activeSong = GameView.Instance.activeSong;

        rect.localPosition = Vector3.zero;
        rect.sizeDelta = Vector2.zero;
        score = GameView.Instance.score;
   
        txtScore.text = score.ToString();
        

        
       
        PlayerPrefs.Save();
    }
    #region Button Action
    public void OnReplayButton()
    {
        GameView.Instance.Replay();
       // AdsManager.Instance.ShowInterstitial();
    }
    public void OnHomeButton()
    {
        Setup(false);

        Home.Instance.Setup(true);
        Home.Instance.ShowSongList();
       

    }
    public void OnNextButton()
    {

        Setup(false);
        GameView.Instance.Setup(true);
        GameView.Instance.NextSong();
        
       
    }
    #endregion
}
