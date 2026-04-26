using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public static Home Instance;
    public GameObject rowPrefab;
    public Transform rowHolder;
    public Image imgSettingOnOff;
    public Transform transSongList, transSetting, transMusic, transGameMode, logo;

    public Sprite sprOn, sprOff;


    List<MSong> songList;
    List<Row> rows;
    RectTransform rect;



    #region Unity Scripts
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        rect = GetComponent<RectTransform>();

        songList = AudioController.Instance.GetSongs();
        CreateSongListRow();
    }
    #endregion

    #region Public Methods
    public void Setup(bool isShow)
    {

        if (rect == null)
            rect = GetComponent<RectTransform>();
        if (isShow)
        {

            gameObject.SetActive(true);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = Vector2.zero;

        }
        else
        {
            gameObject.SetActive(false);
        }


    }

    public void ShowSongList()
    {
        logo.gameObject.SetActive(true);
        transSetting.gameObject.SetActive(false);
        transSongList.gameObject.SetActive(true);
        transMusic.gameObject.SetActive(false);
        transGameMode.gameObject.SetActive(false);

    }
    public void ShowGameMode()
    {
        transGameMode.gameObject.SetActive(true);
        transSongList.gameObject.SetActive(false);
        transMusic.gameObject.SetActive(false);
    }
    public void OnBombMode()
    {
        GameController.Instance.isBombMode = true;

        Invoke("OpenGameView", 0.01f);

    }
    public void OnNormalMode()
    {

        GameController.Instance.isBombMode = false;

        Invoke("OpenGameView", 0.01f);

    }
    void OpenGameView()
    {
        Home.Instance.Setup(false);

        GameView.Instance.Setup(true);
        GameView.Instance.SetSong(GameController.SONG_NAME);
    }
    public void ShowSetting()
    {
        logo.gameObject.SetActive(true);
        transSetting.gameObject.SetActive(true);
        transSongList.gameObject.SetActive(false);
        transMusic.gameObject.SetActive(false);
        transGameMode.gameObject.SetActive(false);


        imgSettingOnOff.sprite = AudioController.Instance.isSoundOff ? sprOff : sprOn;
    }
    public void ShowMusic()
    {
        logo.gameObject.SetActive(false);
        transSetting.gameObject.SetActive(false);
        transSongList.gameObject.SetActive(false);
        transMusic.gameObject.SetActive(true);
        transGameMode.gameObject.SetActive(false);

        MusicView.Instance.Setup(true);
        AudioController.Instance.Play(AudioController.SOUND_MENU);
    }


    public void UpdateSong(MSong song)
    {
        Row row = rows.Find(x => x.name == song.name);
        row.RowUpdate(song);

    }


    public void OnSettingOnOff()
    {
        AudioController.Instance.isSoundOff = !AudioController.Instance.isSoundOff;

        // Cập nhật giao diện
        imgSettingOnOff.sprite = AudioController.Instance.isSoundOff ? sprOff : sprOn;

        // Quan trọng: Gọi hàm xử lý âm thanh ngay lập tức
        AudioController.Instance.UpdateSoundSetting();
    }

    #endregion

    #region Private Methods

    void CreateSongListRow()
    {
        rows = new List<Row>();
        for (int i = 0; i < songList.Count; i++)
        {
            GameObject go = Instantiate(rowPrefab, rowHolder);
            Row row = go.GetComponent<Row>();
            row.txtSerial.text = (i + 1).ToString();
            row.name = songList[i].name;
            row.RowUpdate(songList[i]);
            rows.Add(row);
        }

    }

    #endregion
}