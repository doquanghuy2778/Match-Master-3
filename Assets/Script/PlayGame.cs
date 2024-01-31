using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGame : MonoBehaviour
{
    public Button ButtonNC, ButtonLight;
    private static PlayGame instance;
    public static PlayGame Instance { get => instance; }
    public bool IsActiveItem, IsPlay;
    public Image Button, IconNC, IconLight;
    public GameObject IconLockNC, IconLockLight;
    public TextMeshProUGUI Leveltext;
    public Image CheckNC, CheckLight;
    private int level;
    public GameObject SlotCountNC, SlotDoneNC, SlotCountLight, SlotDoneLight;
    public bool IsActiveNC;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ButtonNC.onClick.AddListener(CheckOnClickNC);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        IsPlay = true;
    }

    private void Update()
    {
        level = PlayerPrefs.GetInt("level");
        if (level == 0)
        {
            Leveltext.SetText("Level 1");
        }
        else
            Leveltext.SetText("Level " + level);

        if (level >= 8)
        {
            IconLockNC.SetActive(false);
            IconNC.enabled = true;
            ButtonNC.enabled = true;
            SlotCountNC.SetActive(true);
        }
        else
        {
            SlotCountNC.SetActive(false);
            IconLockNC.SetActive(true);
            IconNC.enabled = false;
            ButtonNC.enabled = false;
        }
    }

    public void CheckOnClickNC()
    {
        if (level >= 8)
        {
            IsActiveNC = true;
            CheckNC.enabled = true;
            SlotCountNC.SetActive(false);
            SlotDoneNC.SetActive(true);
        }
    }
    public void RemoveDetail()
    {
        IsActiveItem = false;
        IsPlay = false;
        CheckNC.enabled = false;
        SlotCountNC.SetActive(true);
        SlotDoneNC.SetActive(false);
    }
}
