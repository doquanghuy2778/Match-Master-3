using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image CountDown;
    public float TimeInGame;
    public Button NamCham;
    public GameObject CountNC;
    public TextMeshProUGUI TextNC;
    public List<GameObject> ListTarget;
    public List<GameObject> Border;
    public GameObject Target1, Target2;

    private void Start()
    {
        GenPos();
    }

    private void Update()
    {
        if (GameManager.Instance.DemDelete < 1)
            CountDown.fillAmount = 0;
        else
            UIShowSliderTime();

        //khi co ads can fix lai
        if (PlayGame.Instance.IsActiveNC)
        {
            NamCham.enabled = true;
            CountNC.SetActive(false);
            TextNC.enabled = true;
        }
        else
        {
            NamCham.enabled = false;
            CountNC.SetActive(true);
            TextNC.enabled = false;
        }
        ReButton();
    }

    public void UIShowSliderTime()
    {
        if (!GameManager.Instance.IsGameWin && !GameManager.Instance.IsGameOver)
        {
            float curr = GameManager.Instance.CountDownTime;
            float max = GameManager.Instance.MaxTime;
            CountDown.fillAmount = curr / max;
        }
    }

    public void ButtonNamCham()
    {
        PlayGame.Instance.IsActiveItem = true;
    }

    public void ReButton()
    {
        if (PlayGame.Instance.IsActiveNC)
        {
            if (PlayGame.Instance.IsActiveItem)
            {
                CountNC.SetActive(true);
                TextNC.enabled = false;
            }
        }
    }

    public void GenPos()
    {
        for (int i = 0; i < ListTarget.Count; i++)
        {
            if (i == 2)
            {
                Border[i].transform.localPosition = new Vector3(
                                                ListTarget[i].transform.localPosition.x,
                                                ListTarget[i].transform.localPosition.y - 200f,
                                                ListTarget[i].transform.localPosition.z);
            }
            else
                Border[i].transform.localPosition = ListTarget[i].transform.localPosition;
        }
    }

    public void ButtonContinue()
    {
        int dem = 0;
        var ListItem = GameManager.Instance.Inventory1;
        GameManager.Instance.IsGameOver = false;
        GameManager.Instance.CoutTime = 60;
        GameManager.Instance.IsOverTime = false;
        GameManager.Instance.CountDownTime = GameManager.Instance.MaxTime;
        if (GameManager.Instance.Inventory1.Count >= 7)
        {
            foreach (GameObject item in GameManager.Instance.Inventory1)
            {
                item.GetComponent<BoxCollider>().enabled = true;
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.GetComponent<Rigidbody>().useGravity = true;
                item.GetComponent<Rigidbody>().mass = 20;
                item.transform.DOMove(Target1.transform.position, 0.5f);
                if (dem >= 2)
                {
                    item.transform.DOMove(Target2.transform.position, 0.5f);
                }
                dem++;
            }


            foreach (GameObject item in ListItem)
            {
                GameManager.Instance.ItemSpawn.Add(item);
            }
            GameManager.Instance.Inventory.Clear();
            GameManager.Instance.Inventory1.Clear();
        }
    }
}
