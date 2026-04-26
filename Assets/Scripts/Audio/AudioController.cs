using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioController : MonoBehaviour
{

    public static AudioController Instance;
    [HideInInspector]
    public bool isSoundOff;
    public static string SOUND_MENU = "#a";
    public static string SOUND_DEATH = "#A-1";
    public static string SOUND_GAME_OVER = "g4";

    AudioSource source;
    //string PATH = "Sounds/pianosound/";
    string KEY_SOUND = "sound";
    string IS_FAV = "isFav";

    bool isSync;

    List<Sound> sounds = new List<Sound>();

    [SerializeField]
    List<MSong> songList;

    public AudioClip fullMusic;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        isSoundOff = PlayerPrefs.GetInt(KEY_SOUND, 0) == 1;
        SyncSongs();


    }


    public void Play(string name)
    {
        if (isSoundOff) return;
        if (sounds.Find(s => s.name == name) == null) return;
        AudioClip clip = sounds.Find(s => s.name == name).clip;
        if (clip != null)
            source.PlayOneShot(clip);



    }

    public void Play2(string mulClip)
    {
        if (isSoundOff) return;
        if (!mulClip.Contains("."))
        {

            Play(mulClip);
            return;
        }
        string[] clips = mulClip.Split('.');
        foreach (string clip in clips)
        {
            Play(clip);
        }
    }

    public void PlayFullMusic()
    {
        if (isSoundOff) return;

        for (int i = 0; i < songList.Count; i++)
        {
            if (songList[i].name == GameController.SONG_NAME)
            {
                fullMusic = songList[i].file;
                break;
            }
        }

        if (fullMusic == null) return;

        source.clip = fullMusic;
        source.Stop();
        source.Play();
    }
    public void UpdateSoundSetting()
    {
        PlayerPrefs.SetInt(KEY_SOUND, isSoundOff ? 1 : 0);
        PlayerPrefs.Save();

        // Xử lý âm thanh ngay khi toggle
        if (isSoundOff)
        {
            Stop();           // Dừng nhạc nền
                              // Pause() nếu bạn muốn tạm dừng thay vì dừng hẳn
        }
        else
        {
            // Nếu đang ở trong game và muốn tiếp tục nhạc
            if (GameView.Instance != null && GameView.Instance.isStared)
            {
                Resume();
            }
        }
    }
    public void UpdateSetting()
    {
        UpdateSoundSetting();   // gọi hàm mới
    }

    public void Stop()
    {
        source.Stop();
    }
    public void Pause()
    {
        source.Pause();
    }
    public void Resume()
    {

        if (source != null && !source.isPlaying)
            source.UnPause();

    }
    public MSong GetSong(string name)
    {
        MSong song = songList.Find(x => x.name == name);
        if (song == null) return null;
        song.isUnlock = true;
        int fav = PlayerPrefs.GetInt(IS_FAV + name, -1);
        if (fav > -1) song.fav = fav;

        return song;
    }
    public List<MSong> GetSongs()
    {
        if (!isSync)
            SyncSongs();
        return songList;
    }

    private void SyncSongs()
    {
        for (int i = 0; i < songList.Count; i++)
        {
            string name = songList[i].name;
            songList[i].id = i + 1;
            songList[i].isUnlock = true;
            GetSong(name);
        }
        isSync = true;

    }
}
