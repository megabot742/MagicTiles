using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public CanvasScaler canvasScaler;

    [HideInInspector]
    public bool isBombMode;

    public static string SONG_NAME;
    public static int SONG_ID;

    const string IS_FAST = "isFast";
    const string IS_FAV = "isFav";
    const string BEST_SCORE = "bestScore";

    int bestScore;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        GameView.Instance.Setup(false);
        GameOverMenu.Instance.Setup(false);
        Home.Instance.Setup(true);
        Home.Instance.ShowSongList();

		

    }
    public void UpdateCanvas(int value)
    {
        canvasScaler.matchWidthOrHeight = value;
    }
    public void OnHomeButton()
    {
        GameView.Instance.Setup(false);
        GameOverMenu.Instance.Setup(false);
        Home.Instance.ShowSongList();
        AudioController.Instance.Play(AudioController.SOUND_MENU);
    }
    public void OnSettingButton()
    {
        GameView.Instance.Setup(false);
        GameOverMenu.Instance.Setup(false);
        Home.Instance.ShowSetting();
        AudioController.Instance.Play(AudioController.SOUND_MENU);

    }
    public void OnMusicButton()
    {
        GameView.Instance.Setup(false);
        GameOverMenu.Instance.Setup(false);
        Home.Instance.ShowMusic();
        AudioController.Instance.Play(AudioController.SOUND_MENU);
    }
    public bool IsFastOpen()
    {
        return PlayerPrefs.GetInt(IS_FAST, 1) == 1;
    }


    public void SetSongFav(string name, bool isFav)
    {
        MSong s = AudioController.Instance.GetSong(name);
        s.fav = isFav ? 1 : 0;
        Home.Instance.UpdateSong(s);
        if (MusicView.Instance != null)
            MusicView.Instance.UpdateSong(s);
        //SaveSongList();
        PlayerPrefs.SetInt(IS_FAV + name, isFav ? 1 : 0);
        PlayerPrefs.Save();
    }

    public int GetBestScore(string name, int score)
    {
        bestScore = PlayerPrefs.GetInt(BEST_SCORE + name);
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BEST_SCORE + name, bestScore);
            PlayerPrefs.Save();
        }

        return bestScore;
    }
    public bool IsFavSong(string name)
    {
        return PlayerPrefs.GetInt(IS_FAV + name) == 1;
    }
}
