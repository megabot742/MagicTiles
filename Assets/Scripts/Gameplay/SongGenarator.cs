using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongGenarator
{

    const string TONES_HAPPY_BIRTHDAY11 = "d2,d2,e2,d2,g2,#f2,d2,d2,e2,d2,a2,g2,d2,d2,d3,b2,g2,#f2,e2,c3,c3,b2,g2,a2,g2,d2,d2,e2,d2,g2,#f2,d2,d2,e2,d2,a2,g2,d2,d2,d3,b2,g2,#f2,e2,c3,c3,b2,g2,a2,g2";
    const string TONES_HAPPY_BIRTHDAY12 = "T,g,b.d1.g1,b.d1.g1,a,d1.#f1,d1.#f1,g,b.d1.g1,d1.#f1,g,b.d1.g1,b.d1.g1,G-1,d1.g1.b1,d1.g1.b1,c1,e1.g1.c2,e1.g1.c2,g,b.d1.g1,b.d1.g1,g,b.d1,d1.#f1,g,b.d1.g1,b.d1.g1,a,d1.#f1,d1.#f1,g,b.d1.g1,d1.#f1,g,b.d1.g1,b.d1.g1,G-1,d1.g1.b1,d1.g1.b1,c1,e1.g1.c2,e1.g1.c2,g,b.d1.g1,a.d1.#f1,G-1,b.d1.g1,g";
    const string TONES_LITTLE_STAR = "c2,e1,g1,c2,c1,g2,g1,b1,g2,c2,a2,a1,e2,a2,c2,g2,g1,c2,f2,d2,f2,c2,e2,a1,e2,f1,d2,d1,g1,d2,f1,b1,e1,c2,g1,c2,d2,g2,c2,g2,f2,a1,b1,f2,c2,e2,c2,e2,a1,g1,d2,b1,a1,d2,g1,g2,c2,d2,g2,e2,c2,f2,a1,c2,f2,c2,e2,c2,e2,d2,g1,b1";    
    const string TONES_HAPPY_NEW_YEAR = "d1,d1,d1,a,#f1,#f1,#f1,d1,d1,#f1,a1,a1,g1,#f1,e1,e1,#f1,g1,g1,#f1,e1,#f1,d1,d1,#f1,e1,a,#c1,e1,d1,e2,#f2,g2,g2,#f2,e2,#f2,d2,d2,#f2,e2,a1,#c2,e2,d2";
    const string TONES_JINLGE_BELLS = "#a2,#a2,#a2,#a2,#a2,#a2,#a2,#c3,#f2,#g2,#a2,b2,b2,b2,b2,b2,#a2,#a2,#a2,#g2,#g2,#f2,#g2,#c3,#a2,#a2,#a2,#a2,#a2,#a2,#a2,#c3,#f2,#g2,#a2,b2,b2,b2,b2,b2,#a2,#a2,#c3,#c3,b2,#g2,#f2";
    const string TONES_BEYER = "#a2,#a2,#a2,#a2,#a2,#a2,#a2,#c3,#f2,#g2,#a2,b2,b2,b2,b2,b2,#a2,#a2,#a2,#g2,#g2,#f2,#g2,#c3,#a2,#a2,#a2,#a2,#a2,#a2,#a2,#c3,#f2,#g2,#a2,b2,b2,b2,b2,b2,#a2,#a2,#c3,#c3,b2,#g2,#f2";

    const string TYPES_HAPPY_BIRTHDAY ="L,L,K,K,K,J,L,L,K,K,K,J,L,L,K,K,K,K,K,L,L,K,K,K,J,L,L,K,K,K,J,L,L,K,K,K,J,L,L,K,K,K,K,K,L,L,K,K,K,JK";
    const string TYPES_LITTLE_STAR = "L,L,L,L,L,L,L,L,L,L,L,L,L,L,K,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L";
    const string TYPES_HAPPY_NEW_YEAR = "L,L,K,K,L,L,K,K,L,L,K,K,L,L,J,L,L,K,K,L,L,K,K,L,L,K,K,L,L,J,L,L,K,K,L,L,K,K,L,L,K,K,L,L,J";
    const string TYPES_JINLGE_BELLS = "L,L,K,L,L,K,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,K,L,L,K,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,L,K";
    const string TYPES_BEYER = "M,M,L,L,L,L,M,M,L,L,L,L,M,M,L,L,L,L,L,L,L,L,M,M,L,L,L,L,M,M,L,L,L,L,M,M,L,L,L,L,L,L,L,L,M,M,L,L,L,L,L,M,M,L,L,L,L,M,M,L,L,L,L,L,L,L,L,L,L,L,M,M,L,L,L,L,L,M,M,L,L,L,L,L,M,M,L,L,L,L,L,L,L,L,L,L,L,M,M";
    
    public   const int ID_MOZRAT = 1;
    public const int ID_LITTLE_STAR = 2;
    public  const int ID_HAPPY_BIRTHDAY = 3;
    public const int ID_JINGLE_BELLS = 4;
    public const int ID_MERRY_CRISTMAS = 5;
    public const int ID_BEYER = 6;


    

    #region public methods

    public MSongModel GetSong(int songId)
    {
        MSongModel song = new MSongModel();

        song.id = songId;
        //song.songInfo = Home.Instance.GetSong(songId);
        song.types = GetTypes(songId);
        song.tones = GetTones(songId);
        song.tones2 = GetTones2(songId);
        switch (songId)
        {
            case ID_MOZRAT:               
                song.initialSpeed = 1100f;
                song.minForUnlock = 30;
                break;
            case ID_LITTLE_STAR:               
                song.initialSpeed = 1100f;
                song.minForUnlock = 30;
                break;
            case ID_HAPPY_BIRTHDAY:
                song.initialSpeed = 1100f;
                song.minForUnlock = 30;
                break;
            case ID_JINGLE_BELLS:
                song.initialSpeed = 1100f;
                song.minForUnlock = 30;
                break;
            default:
                song.initialSpeed = 1100f;
                song.minForUnlock = 30;
                break;
        }
      

        return song;
    }

    #endregion

    #region private methods


    List<string> GetTones(int songId)
    {
        List<string> tones = new List<string>();

        string[] toneTags = null;

        switch (songId)
        {
            case ID_MOZRAT:
                toneTags = TONES_HAPPY_NEW_YEAR.Split(',');             

                break;
            case ID_HAPPY_BIRTHDAY:
                toneTags = TONES_HAPPY_BIRTHDAY11.Split(',');
                break;
            case ID_LITTLE_STAR:
                toneTags = TONES_LITTLE_STAR.Split(',');
                break;
            case ID_JINGLE_BELLS:
                toneTags = TONES_JINLGE_BELLS.Split(',');
                break;
            case ID_BEYER:
                toneTags = TONES_BEYER.Split(',');
                break;
        }

        for (int i = 0; i < toneTags.Length; i++)
        {
            tones.Add(toneTags[i]);
        }
      

        return tones;
    }
    List<string> GetTones2(int songId)
    {
        List<string> tones = new List<string>();

        string[] toneTags = null;

        switch (songId)
        {
            case ID_MOZRAT:
                toneTags = TONES_HAPPY_NEW_YEAR.Split(',');

                break;
            case ID_HAPPY_BIRTHDAY:
                toneTags = TONES_HAPPY_BIRTHDAY12.Split(',');
                break;
            case ID_LITTLE_STAR:
                toneTags = TONES_LITTLE_STAR.Split(',');
                break;
            case ID_JINGLE_BELLS:
                toneTags = TONES_JINLGE_BELLS.Split(',');
                break;
            case ID_BEYER:
                toneTags = TONES_BEYER.Split(',');
                break;
        }


        for (int i = 0; i < toneTags.Length; i++)
        {
            tones.Add(toneTags[i]);
        }


        return tones;
    }

    private Node.Type GetType(string typeString)
    {
        Node.Type type = Node.Type.NORMAL;

        switch (typeString)
        {
            case "S":
                type = Node.Type.START;
                break;
            case "L":
                type = Node.Type.NORMAL;
                break;
            case "K":
                type = Node.Type.LONG;
                break;
            case "J":
                type = Node.Type.LONG2;
                break;
            case "JK":
                type = Node.Type.LONG3;
                break;
            case "M":
                type = Node.Type.MIXED;
                break;
        }

        return type;
    }

    List<Node.Type> GetTypes(int songId)
    {

        string typeString = "";

        switch (songId)
        {
            case ID_MOZRAT:
                typeString = TYPES_HAPPY_NEW_YEAR;
              
                break;
            case ID_HAPPY_BIRTHDAY:
                typeString = TYPES_HAPPY_BIRTHDAY;
                break;
            case ID_LITTLE_STAR:
                typeString = TYPES_LITTLE_STAR;
                break;
            case ID_JINGLE_BELLS:
                typeString = TYPES_JINLGE_BELLS;
                break;
            case ID_BEYER:
                typeString = TYPES_BEYER;
                break;
        }

        List<Node.Type> types = new List<Node.Type>();
        string[] typeList = typeString.Split(',');
        for (int i = 0; i < typeList.Length; i++)
        {

            types.Add(GetType(typeList[i]));
        }


        return types;
    }


    #endregion


}
