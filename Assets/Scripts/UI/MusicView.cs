using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicView : MonoBehaviour
{

    public static MusicView Instance;
    public GameObject rowPrefab;
    public Transform rowHolder;
    public Transform activeSong, activeFav;

    List<MSong> songList;
    List<Row> rows;

    void Awake()
    {
        Instance = this;
      
    }
    
    public void Setup(bool isShow)
    {
      
        transform.gameObject.SetActive(isShow);
        if (songList == null || songList!=null && songList.Count<1)
        {
            songList = AudioController.Instance.GetSongs();
            CreateSongListRow();
        }
    }
    public void UpdateSong(MSong song)
    {
        Row row = rows.Find(x => x.name == song.name);
        if (row != null)
            row.RowUpdate(song);
    }

    public void OnClickSongs()
    {

        activeFav.gameObject.SetActive(false);
        activeSong.gameObject.SetActive(true);
        for (int i = 0; i < rowHolder.childCount; i++)
        {
            rowHolder.GetChild(i).gameObject.SetActive(true);
        }
    }
    public void OnClickFav()
    {

        activeFav.gameObject.SetActive(true);
        activeSong.gameObject.SetActive(false);
        for (int i = 0; i < rowHolder.childCount; i++)
        {
            if (!rows[i].isFav)
                rowHolder.GetChild(i).gameObject.SetActive(false);
        }
    }


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
}
