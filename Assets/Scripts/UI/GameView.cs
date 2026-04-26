using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public static GameView Instance;


    public GameObject nodePrefab, boundary;
    public Transform nodeHook;
    public Transform transContinue;
    public Transform transBestScore;

    RectTransform rectTrans;

    public TMP_Text txScore, txtCountDown, txtTitle, txtType, txtBestScore;
    public float speed = 0f;
    public float speedIncrement = 300f;
    private int lastBackgroundScore = 0;// Điểm lần cuối đổi background
    private const int SCORE_PER_LEVEL = 30;
    public float intervalForBg = 10f;
    public bool isDead = false;
    public bool isStared, isTap = true;
    bool isPaused;


    float nodeWidth = 200f;
    //float nodeHeight = 300f;

    float screenHeight, screenWidth, devidedValue;


    Node lastNode, deadNode;
    public int score;
    int rndIndex, nodeIndex;
    int songIndex;

    Transform nodeHolder, boundaryHolder;


    public MSong activeSong;
    public List<Node> activeNodes = new List<Node>();
    public Image background;
    public List<Sprite> backgroundImgList;

    int mixedNodeCount, spwanCompleted;
    int songId;
    float lastMixedNodePosY = 0;

    IEnumerator ienChangeBg;
    int bgCount;



    #region Unity Scripts
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        devidedValue = screenWidth / 4;


        PauseScreen.Instance.Setup(false);
    }

    void Update()
    {
        if (isStared && speed > 0)
        {
            transBestScore.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);
        }
    }

    #endregion

    #region Public Methods

    public float getSpeed()
    {
        return speed;
    }
    public void Setup(bool isShow)
    {

        if (rectTrans == null)
            rectTrans = GetComponent<RectTransform>();

        if (!isShow)
        {
            gameObject.SetActive(false);
        }
        else
        {

            gameObject.SetActive(true);
            rectTrans.localPosition = Vector3.zero;
            transContinue.gameObject.SetActive(false);
        }



    }

    public void SetSong(string songName)
    {
        songIndex = GameController.SONG_ID - 1;

        Reset();
        ScoreUpdate(score);

        Generate();
        MakeBoundary();
        print("Set song:" + songIndex);
    }
    public void NextSong()
    {
        songIndex++;

        if (songIndex >= AudioController.Instance.GetSongs().Count || !AudioController.Instance.GetSongs()[songIndex].isUnlock)
        {
            songIndex = 0;
        }



        GameController.SONG_NAME = AudioController.Instance.GetSongs()[songIndex].name;
        GameController.SONG_ID = AudioController.Instance.GetSongs()[songIndex].id;
        SetSong(GameController.SONG_NAME);



    }
    public void Replay()
    {
        Home.Instance.Setup(false);
        GameOverMenu.Instance.Setup(false);
        OnPause(false);
        Setup(true);
        Reset();
        ScoreUpdate(score);
        Generate();

    }

    public void OnTap()
    {

        if (!isStared || isDead || !isTap || isPaused) return;

        float clickPosX = Input.mousePosition.x;
        float clickPosY = Input.mousePosition.y;


        for (int i = 0; i < activeNodes.Count; i++)
        {
            if (activeNodes[i].transform.localPosition.y < clickPosY && clickPosY < (activeNodes[i].height + activeNodes[i].transform.localPosition.y))
            {

                deadNode = activeNodes[i];
                int index = (int)(clickPosX / devidedValue);
                Vector3 deadPos = new Vector3(index * nodeWidth, activeNodes[i].transform.localPosition.y, 0);
                speed = 0;

                Node dn = DeadNodeGenerate(deadPos);
                StartCoroutine(IDeadNode(dn, true));
                return;
            }
        }



    }

    public void OnPause(bool isTrue)
    {
        PauseScreen.Instance.Setup(isTrue);
        isPaused = true;
    }

    public void Dead(Node deadNode)
    {
        this.deadNode = deadNode;
        StartCoroutine(IDeadNode(deadNode, false));
    }
    public void OnLeave(Node node, bool isSucceed)
    {

        if (!isSucceed)
        {
            deadNode = node;

            StartCoroutine(IDeadNode(node, false));

        }
        else if (isSucceed)
        {

            if (nodeIndex < activeSong.totalNote)
                GenerateSingle(nodeIndex);
            else
            {

                nodeIndex = 0;
                // speed += 200f;
                spwanCompleted++;
            }
        }
    }
    void PlayMusic(int toneIndex)
    {
        if (!isStared) return;
        isPaused = false;
        AudioController.Instance.Resume();
        //string note = activeSongModel.tones[toneIndex % activeSongModel.tones.Count];
        //string note2 = activeSongModel.tones2[toneIndex % activeSongModel.tones2.Count];
        //AudioController.Instance.Play(note);
        //AudioController.Instance.Play2(note2);


    }

    public void StartGame()
    {
        isStared = true;
        activeSong = AudioController.Instance.GetSong(GameController.SONG_NAME);

        // Tốc độ khởi đầu
        speed = activeSong.tempo * screenHeight / 2560f;
        lastBackgroundScore = 0;           // Reset khi bắt đầu bài mới
        Invoke("ChangeBackground", intervalForBg);   // Background đầu tiên

    }

    public void ScoreUpdate(int amount)
    {
        score += amount;
        txScore.text = score.ToString();

        CheckSpeedAndBackground();
    }
    private void CheckSpeedAndBackground()
    {
        // Tăng tốc mỗi SCORE_PER_LEVEL điểm
        if (score >= lastBackgroundScore + SCORE_PER_LEVEL)
        {
            // Tăng tốc độ
            speed += speedIncrement;

            // Đổi background
            ChangeBackground();

            // Cập nhật mốc điểm tiếp theo
            lastBackgroundScore = (score / SCORE_PER_LEVEL) * SCORE_PER_LEVEL;

            Debug.Log($"Level Up! Speed = {speed} | Score = {score}");
        }
    }

    public bool isMoveable()
    {
        return !isDead && !isPaused && isStared;
    }

    public void OnWatchAd()
    {

    }

    #endregion

    #region Private Methods

    void ChangeBackground()
    {
        if (isPaused || isDead || !isStared) return;

        if (ienChangeBg != null)
            StopCoroutine(ienChangeBg);
        ienChangeBg = IChangeBackground(backgroundImgList[bgCount]);
        StartCoroutine(ienChangeBg);
        bgCount++;
        if (bgCount >= backgroundImgList.Count)
        {
            bgCount = 0;
        }

    }

    IEnumerator IChangeBackground(Sprite img)
    {

        Color c = background.color;

        while (c.a > 0.1f)
        {
            c.a -= 0.01f;
            yield return null;
            background.color = c;
        }
        background.sprite = img;
        yield return new WaitForSeconds(1);
        while (c.a < 1f)
        {
            c.a += 0.01f;
            yield return null;
            background.color = c;
        }
        CancelInvoke("ChangeBackground");
        Invoke("ChangeBackground", intervalForBg);
    }

    IEnumerator IDeadNode(Node dn, bool isWrongTap)
    {
        isTap = false;
        if (isWrongTap)
        {
            speed = 0;
        }

        else
        {
            speed = -speed;
            yield return new WaitForSeconds(0.5f);
            speed = 0;
            dn.gameObject.SetActive(false);

            dn = DeadNodeGenerate(dn.transform.localPosition);
        }

        isDead = true;

        for (int i = 0; i < 4; i++)
        {

            dn.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            dn.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);


        }


        yield return new WaitForSeconds(1f);

        //transContinue.gameObject.SetActive(true);
        //time countdown

        //int time = 5;
        //while (time > 0)
        //{
        //    time--;
        //    txtCountDown.text = time.ToString();
        //    yield return new WaitForSeconds(1f);
        //}

        //transContinue.gameObject.SetActive(false);

        isStared = false;
        Setup(false);


        GameOverMenu.Instance.Setup(true);
    }

    private void MakeBoundary()
    {
        if (boundaryHolder == null)
        {
            boundaryHolder = new GameObject("BoundaryHolder").transform;
            boundaryHolder.parent = nodeHook;
            boundaryHolder.localScale = Vector3.one;
        }
        for (int i = 1; i <= 3; i++)
        {
            GameObject go = Instantiate(boundary);
            go.transform.SetParent(boundaryHolder);
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.pivot = Vector2.zero;
            rect.localPosition = new Vector2(200f * i, 0);
            rect.sizeDelta = new Vector2(1, Screen.height);


        }
    }

    Node DeadNodeGenerate(Vector3 pos)
    {
        AudioController.Instance.Stop();
        AudioController.Instance.Play(AudioController.SOUND_DEATH);
        GameObject go = Instantiate(nodePrefab);
        go.transform.SetParent(nodeHolder);
        go.GetComponent<RectTransform>().localPosition = pos;
        go.GetComponent<RectTransform>().localScale = Vector3.one;


        Node n = go.GetComponent<Node>();
        n.Init(deadNode.type, deadNode.nodeIndex, true);
        GameController.Instance.GetBestScore(GameController.SONG_NAME, score);

        return n;



    }
    void Generate()
    {

        if (nodeHolder == null)
        {
            nodeHolder = new GameObject("NodeHolder").transform;
            nodeHolder.parent = nodeHook;
            nodeHolder.localScale = Vector3.one;
        }

        for (int i = -1; i < 8; i++)
        {
            GenerateSingle(i);
        }
    }

    void Reset()
    {
        lastBackgroundScore = 0;
        screenHeight = Screen.height;
        //nodeHeight = 300;
        nodeWidth = 200;

        score = 0;
        nodeIndex = 0;
        lastMixedNodePosY = 0;
        isStared = false;
        isDead = false;
        isPaused = false;
        isTap = true;

        if (nodeHolder != null)
            Destroy(nodeHolder.gameObject);

        nodeHolder = null;
        activeNodes.Clear();
        Color c = background.color;
        c.a = 1;
        background.color = c;
        UpdateBestScreen();
    }
    void UpdateBestScreen()
    {
        MSong song = AudioController.Instance.GetSong(GameController.SONG_NAME);
        transBestScore.GetComponent<RectTransform>().position = Vector3.zero;
        txtTitle.text = song.name;
        txtType.text = song.type;
        txtBestScore.text = GameController.Instance.GetBestScore(GameController.SONG_NAME, score).ToString();
    }

    Node.Type GetRndType()
    {
        Node.Type type = Node.Type.NORMAL;

        int v = Random.Range(0, 4);
        if (v == 0)
        {
            int t = Random.Range(0, 3);
            if (t == 0 && GameController.Instance.isBombMode)
                type = Node.Type.BOMB;
            else
                type = Node.Type.NORMAL;
        }
        else if (v == 1)
        {
            type = Node.Type.LONG;
        }
        else if (v == 2)
        {
            type = Node.Type.LONG2;
        }
        else if (v == 3)
        {
            type = Node.Type.LONG3;
        }
        else if (v == 4 && GameController.Instance.isBombMode)
        {
            type = Node.Type.BOMB;
        }
        return type;
    }

    void GenerateSingle(int index)
    {

        Node.Type activeType = index == -1 ? Node.Type.START : GetRndType();



        float xPos = nodeWidth * GetUniqueRND();
        float yPos = 0;

        if (activeType == Node.Type.START)
        {
            yPos = transBestScore.GetComponent<RectTransform>().sizeDelta.y;
        }
        else if (activeType == Node.Type.MIXED)
        {
            mixedNodeCount++;
            if (mixedNodeCount >= 2)
            {
                yPos = lastMixedNodePosY;
                mixedNodeCount = 0;
            }
            else
            {
                yPos = lastNode.transform.localPosition.y + lastNode.height;
                lastMixedNodePosY = yPos;
            }
        }
        else
        {
            yPos = lastNode.transform.localPosition.y + lastNode.height;

        }
        Vector3 pos = new Vector3(xPos, yPos, 0);
        GameObject go = Instantiate(nodePrefab);
        go.SetActive(true);
        go.transform.SetParent(nodeHolder);
        go.GetComponent<RectTransform>().localPosition = pos;
        go.GetComponent<RectTransform>().localScale = Vector3.one;
        //go.transform.localScale = Vector3.one;


        lastNode = go.GetComponent<Node>();
        lastNode.Init(activeType, index, false);
        if (index > -1)
            lastNode.OnClicked += PlayMusic;
        lastNode.OnLeave += OnLeave;
        activeNodes.Add(lastNode);

        nodeIndex++;



    }

    int GetUniqueRND()
    {
        int tmpIndex = Random.Range(0, 4);
        while (rndIndex == tmpIndex)
        {
            tmpIndex = Random.Range(0, 4);
        }
        rndIndex = tmpIndex;
        return rndIndex;
    }


    #endregion
}