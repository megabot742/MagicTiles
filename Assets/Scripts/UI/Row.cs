using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour
{
    public Text txtSerial, txtTitle, txtType;

    public Image bg, imgFav;
    public Sprite sprFavActive, sprFavInactive,sprLock,sprNormal;
    public bool isFav;
    public string songName;
    public int id;

    void Start()
    {

    }
    public void RowUpdate(MSong song)
    {
        txtTitle.text = song.name;
        txtType.text = song.type;
        isFav = song.fav == 1;
        songName = song.name;
        
        id = song.id;
        imgFav.sprite = isFav ? sprFavActive : sprFavInactive;
        bg.sprite = sprNormal;
        
    }
    public void OnPlayClick()
    {
        GameController.SONG_NAME = songName;
        GameController.SONG_ID = id;

        GameController.Instance.UpdateCanvas(0);
        
        Home.Instance.Setup(true);     
        Home.Instance.ShowGameMode(); // Hiện chọn chế độ (Normal / Bomb)
    }
    

    public void OnFavClick()
    {       
        isFav = !isFav;
        imgFav.sprite = isFav ? sprFavActive : sprFavInactive;
        GameController.Instance.SetSongFav(songName, isFav);
    }
}