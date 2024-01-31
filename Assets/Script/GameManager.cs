using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //danh sach item ban dau
    [SerializeField] private List<GameObject> _items;
    //danh sach cac diem tren man hinh de obj spawn
    public List<RectTransform> points;
    public List<Transform> TargetPosition;
    //List item da duoc spawn
    public List<GameObject> ItemSpawn;
    //List item chua dc sap xep
    public List<GameObject> Inventory;
    //List item da duoc sap xep
    public List<GameObject> Inventory1;
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    public bool IsGameOver, IsGameWin;
    public GameObject GameOverPanel, GameWinPanel;
    public float CountDownTime;
    public float MaxTime;
    public bool IsDelete, IsOverTime;
    private List<Tween> _tweens = new List<Tween>();
    public int DemDelete = 0;
    public float DelayTime;
    public float LastTimeCall;
    public int currLevel, Minue, Second;
    public float CoutTime;
    public TextMeshProUGUI TextTime, Level;
    [SerializeField] private float _thrush;
    public List<GameObject> Items;
    public GameObject NamCham;
    public Button ButtonGameWin;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CoutTime = 0;
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 144;
        if (PlayerPrefs.GetInt("level") != 0)
        {
            currLevel = PlayerPrefs.GetInt("level");
        }
        else
        {
            currLevel = SaveGame.Instance.CurrLevel;
        }
        SpawnItem();
        LastTimeCall = DelayTime / 2;
        CountDownTime = MaxTime;
        CoutTime = 60;
        Level.SetText("Level " + currLevel);
    }

    private void Update()
    {
        if (Screen.orientation != ScreenOrientation.Portrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        CountDownTimer();
        if (DemDelete >= 1)
            FillTimer();
        if (PlayGame.Instance.IsActiveItem && PlayGame.Instance.IsPlay)
            NamCham.SetActive(true);
        else
            NamCham.SetActive(false);
    }

    private void SpawnItem()
    {
        float minX = points[0].position.x;
        float maxX = points[1].position.x;
        int a = currLevel;
        for (int i = 0; i < a; i++)
        {
            int randomItem;
            if (currLevel == 1)
            {
                randomItem = i;
            }
            else
            {
                randomItem = Random.Range(0, _items.Count);
            }
            for (int j = 0; j < 3; j++)
            {
                GameObject item = _items[randomItem];
                Vector3 spawnPosition = new Vector3(Random.Range(minX / 2, maxX / 2),
                                                    Random.Range(20, 25),
                                                    Random.Range(-20, 32));
                GameObject gameObject = Instantiate(item, spawnPosition, Quaternion.identity);
                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * _thrush, ForceMode.Impulse);
                gameObject.SetActive(true);
                ItemSpawn.Add(gameObject);
            }
        }
    }

    private void SortListItem()
    {
        if (Inventory.Count > 0)
        {
            //dua cac item co gia tri bang nhau vao 1 nhom (3 item)
            var listItem = Inventory.OrderBy(a => a.GetComponent<Items>().Value).ToList();
            Inventory1.Clear();
            foreach (var a in listItem)
            {
                Inventory1.Add(a);
            }
            StartCoroutine(DeleteItem());
        }
        else
            return;
    }

    //xoa 3 item lien tiep dung canh nhau
    private IEnumerator DeleteItem()
    {
        yield return new WaitForSeconds(0.01f);
        var listObject = from obj in Inventory1 group obj by obj.GetComponent<Items>().Value;
        foreach (var group in listObject)
        {
            if (group.Count() == 3)
            {
                foreach (var objs in group)
                {
                    DeleteObj(objs);
                }
                IsDelete = true;
                DemDelete++;
            }
        }
    }

    private void DeleteObj(GameObject a)
    {
        Inventory1.Remove(a);
        Inventory.Remove(a);
        Destroy(a, 0.6f);
        Show();
    }

    public void Pick(GameObject a)
    {
        Inventory.Add(a);
        ItemSpawn.Remove(a);
        SortListItem();
        a.GetComponent<Rigidbody>().useGravity = false;
        if (a.GetComponent<BoxCollider>() != null)
        {
            a.GetComponent<BoxCollider>().enabled = false;
        }
        Vector3 scaleObject = a.transform.localScale;
        Show();
        StartCoroutine(CheckGameOver());
        StartCoroutine(CheckGameWon());
    }

    public void Show()
    {
        int i = 0;
        foreach (Tween item in _tweens)
        {
            item.Kill();
        }
        foreach (GameObject obj in Inventory1)
        {
            Vector3 duration;
            int valueItem = obj.GetComponent<Items>().Value;
            if (valueItem == 3)
            {
                duration = new Vector3(TargetPosition[i].position.x - 2f,
                                            TargetPosition[i].position.y,
                                            TargetPosition[i].position.z - 1f);
            }
            else if (valueItem == 1)
            {
                duration = new Vector3(TargetPosition[i].position.x + 2.5f,
                                            TargetPosition[i].position.y,
                                            TargetPosition[i].position.z);
            }
            else if (valueItem == 6 || valueItem == 11)
            {
                duration = new Vector3(TargetPosition[i].position.x,
                                            TargetPosition[i].position.y,
                                            TargetPosition[i].position.z + 1f);
            }
            else
            {
                duration = new Vector3(TargetPosition[i].position.x,
                                            TargetPosition[i].position.y,
                                            TargetPosition[i].position.z + 0.5f);
            }
            obj.transform.DOMove(duration, 0.5f);
            obj.GetComponent<Rigidbody>().isKinematic = true;
            i++;
        }
    }

    public void RotationItem()
    {
        if (Inventory1.Count > 0)
        {
            foreach (GameObject obj in Inventory1)
            {
                obj.transform.Rotate(0, 0.5f, 0);
            }
        }
    }

    private IEnumerator CheckGameOver()
    {
        yield return new WaitForSeconds(0.3f);
        if (Inventory1.Count >= 7 || IsOverTime)
        {
            IsGameOver = true;
            GameOverPanel.SetActive(true);
            //for (int i = 0; i < Inventory1.Count; i++)
            //{
            //    ScaleObject(Inventory1[i], 1, Inventory1[i].transform.localScale);
            //}
        }
        else
            IsGameOver = false;
        NamCham.SetActive(false);
    }

    private void ScaleObject(GameObject obj, float scale, Vector3 originScale)
    {
        obj.transform.DOScale(scale * originScale, 1f)
                    .OnComplete(() =>
                    {
                        if (scale > 1.1f)
                        {
                            ScaleObject(obj, 1, originScale);
                        }
                        else
                        {
                            ScaleObject(obj, 1.2f, originScale);
                        }
                    });
    }

    public void FillTimer()
    {
        if (!IsGameOver && !IsGameWin)
        {
            CountDownTime -= Time.deltaTime;
            if (IsDelete)
                CountDownTime += MaxTime - CountDownTime;
            if (CountDownTime < 0)
            {
                IsOverTime = true;
                IsGameOver = true;
                StartCoroutine(CheckGameOver());
            }
            else
            {
                IsDelete = false;
                IsOverTime = false;
                IsGameOver = false;
            }
        }
    }

    public void CountDownTimer()
    {
        if (!IsGameOver && !IsGameWin)
        {
            CoutTime -= Time.deltaTime;
            Minue = Mathf.FloorToInt(CoutTime / 60);
            Second = Mathf.FloorToInt(CoutTime % 60);
            TextTime.text = string.Format("{0:00}:{1:00}", Minue, Second);
            if (CoutTime <= 1)
            {
                CoutTime = 0;
                IsGameOver = true;
                GameOverPanel.SetActive(true);
            }
            else
                IsGameOver = false;
        }
    }

    public void CountLevel()
    {
        currLevel++;
        PlayerPrefs.SetInt("level", currLevel);
        PlayerPrefs.Save();
        PlayGame.Instance.RemoveDetail();
    }

    public void NextLevel()
    {
        Inventory1.Clear();
        SpawnItem();
        IsGameWin = false;
        Level.SetText("Level " + currLevel);
        CountDownTime = MaxTime;
        DemDelete = 0;
        CoutTime = 0;
        CoutTime = 60;
        PlayGame.Instance.IsPlay = true;
    }

    public void ReplayLevel()
    {
        Inventory1.Clear();
        Inventory.Clear();
        GameObject[] listGameObject = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in listGameObject)
        {
            Destroy(item);
        }
        DemDelete = 0;
        CountDownTime = MaxTime;
        ItemSpawn.Clear();
        SpawnItem();
        IsOverTime = false;
        IsGameOver = false;
        Level.SetText("Level " + currLevel);
        CoutTime = 0;
        CoutTime = 60;
    }

    private IEnumerator CheckGameWon()
    {
        yield return new WaitForSeconds(0.5f);
        if (ItemSpawn.Count == 0 && Inventory.Count == 0)
        {
            IsGameWin = true;
            if (!IsGameOver)
                GameWinPanel.SetActive(true);
        }
        else
        {
            IsGameWin = false;
        }
    }
}
