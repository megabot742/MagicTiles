using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    //Enum type
    public enum Type { DEAD, START, NORMAL, BOMB, LONG, LONG2, LONG3, MIXED };
    public Type type;

    //UI/Visual node
    [SerializeField] private Image endPointImg;
    [SerializeField] private Sprite normalImg;
    [SerializeField] private Sprite clickedImg;
    [SerializeField] private Sprite startImg;
    [SerializeField] private Sprite longImg;
    [SerializeField] private Sprite deadImg;
    [SerializeField] private TMP_Text txtStart;
    [SerializeField] private TMP_Text txtPopupScore;

    //Action Event
    public System.Action<int> OnClicked;
    public System.Action<Node, bool> OnLeave;

    public float height = 300f, width = 200f;

    //Trạng thái nội bộ Node
    float scoreLong = 0;
    [SerializeField] private bool isClicked;
    [SerializeField] private bool isLongType;
    [SerializeField] private bool isThumps;
    [SerializeField] private bool isChecked;
    [SerializeField] private bool isStarted;
    [SerializeField] private bool isBomb;
    private float initialLongHeight;

    //RectTransform references
    private RectTransform rectTrans;
    private RectTransform longRect;
    private RectTransform txtRect;

    //Transform con bên trong prefab node
    [SerializeField] private Transform longB;
    [SerializeField] private Transform bomb;
    [SerializeField] private Transform longTrans;
    public int nodeIndex;
    private int toneIdex;

    //Độ cao cố định từng node
    private float normalHeight = 300f;
    private float longHeight = 700f;
    private float long2Height = 1100f;
    private float long3Height = 1400f;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (!GameView.Instance.isMoveable()) return;
        Move();
        LongRectSize();
        CheckPos();
        Disable();

    }
    #region Init Node
    float GetHeight(Type type)
    {
        return type switch
        {
            Type.LONG => longHeight,
            Type.LONG2 => long2Height,
            Type.LONG3 => long3Height,
            _ => normalHeight
        };
    }

    public void Init(Type type, int nodeIndex, bool isDead)
    {
        this.type = type;
        this.nodeIndex = nodeIndex;
        this.toneIdex = nodeIndex;
        isBomb = false;
        isLongType = false;

        height = GetHeight(type);

        // Reset tất cả visual về trạng thái ban đầu
        ResetVisuals();

        // Thiết lập theo loại Node
        switch (type)
        {
            case Type.START:
                GetComponent<Image>().sprite = startImg;
                if (txtStart != null) txtStart.gameObject.SetActive(true);
                break;

            case Type.NORMAL:
            case Type.MIXED:
                GetComponent<Image>().sprite = normalImg;
                break;

            case Type.BOMB:
                GetComponent<Image>().sprite = normalImg;
                if (bomb != null) bomb.gameObject.SetActive(true);
                isBomb = true;
                break;

            case Type.LONG:
            case Type.LONG2:
            case Type.LONG3:
                SetupLongNode(type);
                break;
        }

        // Nếu là node chết (dùng khi miss)
        if (isDead)
        {
            SetupDeadNode();
        }

        // Set kích thước chung cho node
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
    private void ResetVisuals()
    {
        if (longTrans != null) longTrans.gameObject.SetActive(false);
        if (longB != null) longB.gameObject.SetActive(false);
        if (bomb != null) bomb.gameObject.SetActive(false);
        if (txtStart != null) txtStart.gameObject.SetActive(false);
        if (endPointImg != null) endPointImg.gameObject.SetActive(false);
    }

    // Thiết lập cho các loại Long Note (LONG, LONG2, LONG3)
    private void SetupLongNode(Type longType)
    {
        isLongType = true;

        if (longTrans != null) longTrans.gameObject.SetActive(true);
        if (longB != null) longB.gameObject.SetActive(true);

        GetComponent<Image>().sprite = longImg;

        longRect = longTrans?.GetComponent<RectTransform>();
        if (longRect != null)
        {
            // Lưu chiều cao ban đầu của thanh Long
            initialLongHeight = height * 0.95f;           // ← Quan trọng
            longRect.sizeDelta = new Vector2(width, 100f);   // Bắt đầu từ height = 0 (rất nhỏ)
        }

        // Perfect zone (endPointImg)
        if (endPointImg != null)
        {
            endPointImg.gameObject.SetActive(true);
            RectTransform imgRect = endPointImg.GetComponent<RectTransform>();
            imgRect.anchoredPosition = new Vector2(0, -height * 0.20f);
        }
    }

    // Thiết lập node chết (khi miss)
    private void SetupDeadNode()
    {
        GetComponent<Image>().sprite = deadImg;
        if (longTrans != null) longTrans.gameObject.SetActive(false);
        if (longB != null) longB.gameObject.SetActive(false);
        if (txtStart != null) txtStart.gameObject.SetActive(false);
    }
    void Move() //Di chuyển node
    {
        transform.Translate(new Vector3(0, -1, 0) * GameView.Instance.getSpeed() * Time.deltaTime);
    }
    #endregion
    #region Click Action
    public void OnClickDown()
    {
        if (isClicked) return;

        isClicked = true;

        switch (type)
        {
            case Type.START:
                ClickedStart();
                break;
            case Type.BOMB:
            case Type.NORMAL:
            case Type.MIXED:
                ClickedNormal();
                break;
            case Type.LONG:
            case Type.LONG2:
            case Type.LONG3:
                StartLongHold();
                break;
        }

        if (GameView.Instance.isStared && OnClicked != null)
            OnClicked(toneIdex);
    }
    public void OnLongImage()
    {
        toneIdex++;

        //note = songGenerator.GetHappyBirthdayNote(nodeIndex);

        if (OnClicked != null)
        {
            OnClicked(toneIdex);
        }
    }

    public void OnClickUp()
    {
        if (!GameView.Instance.isStared) return;

        if (isLongType && isThumps)
        {
            FinishLongNote();
        }
        else
        {
            isThumps = false;
        }
    }
    void ClickedStart()
    {
        isThumps = true;
        if (txtStart != null) txtStart.gameObject.SetActive(false);
        GetComponent<Image>().sprite = clickedImg;
        GameView.Instance.StartGame();
        AudioController.Instance.PlayFullMusic();
    }

    void ClickedNormal()
    {
        if (!GameView.Instance.isStared) return;
        if (isBomb)
        {
            GameView.Instance.Dead(this);
            return;
        }

        GetComponent<Image>().sprite = clickedImg;
        GameView.Instance.ScoreUpdate(1);
    }

    private void StartLongHold()
    {
        if (!GameView.Instance.isStared) return;
        isThumps = true;
        scoreLong = 0f;
    }
    private void FinishLongNote()
    {
        isThumps = false;

        int finalPoints = GetLongNoteScore();

        if (txtPopupScore != null && finalPoints > 0)
        {
            txtPopupScore.text = "+" + finalPoints;
            txtPopupScore.gameObject.SetActive(true);
            StartCoroutine(HidePopupScore(0.8f));
        }

        GameView.Instance.ScoreUpdate(finalPoints);
        scoreLong = 0f;
    }
    private int GetLongNoteScore()
    {
        if (longRect == null) return 1;

        float currentPercent = longRect.sizeDelta.y / height;

        // Perfect range: 78% - 82%
        if (currentPercent >= 0.75f && currentPercent <= 0.85f)
        {
            return 4;   // Perfect
        }
        else if (currentPercent > 0.4f)   // vẫn giữ được một phần
        {
            return 2;   // Good
        }
        else
        {
            return 1;   // Bad
        }
    }

    private IEnumerator HidePopupScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (txtPopupScore != null)
            txtPopupScore.gameObject.SetActive(false);
    }
    #endregion
    #region Size Node
    void LongRectSize()
    {
        if (!isLongType || !isThumps || longRect == null) return;

        // === GIÃN DÀI TỪ DƯỚI LÊN (tăng chiều cao) ===
        float growSpeed = GameView.Instance.getSpeed() * Time.deltaTime * 1.5f;   // Chỉnh tốc độ giãn ở đây

        float currentHeight = longRect.sizeDelta.y;
        float newHeight = Mathf.Min(initialLongHeight, currentHeight + growSpeed);  // Không cho vượt quá chiều cao ban đầu

        longRect.sizeDelta = new Vector2(longRect.sizeDelta.x, newHeight);

        // Tích điểm theo thời gian giữ
        scoreLong += Time.deltaTime * 4f;
    }
    void CheckPos()
    {
        if (isBomb && !isClicked && !isChecked && transform.position.y < -150f)
        {
            if (OnLeave != null)
            {
                OnLeave(this, true);
                isChecked = true;

                return;
            }
        }
        else if (isClicked && !isChecked && transform.position.y < -150f)
        {
            if (OnLeave != null)
            {
                OnLeave(this, true);
                isChecked = true;
                return;
            }

        }
        else if (OnLeave != null && !isClicked && !isChecked && transform.position.y < -150f)
        {
            OnLeave(this, false);
            isChecked = true;
        }
    }

    void Disable()
    {
        if (!isClicked) return;
        if (transform.position.y < -2 * height)
        {
            GameView.Instance.activeNodes.Remove(this);
            Destroy(gameObject);
        }
    }
    #endregion
}