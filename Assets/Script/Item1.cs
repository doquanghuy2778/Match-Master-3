using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item1 : MonoBehaviour
{
    [SerializeField] private GameObject _targetA, _targetB;
    public GameObject Prefab;
    [SerializeField] private List<GameObject> _items;
    private GameObject _itemSpawn;
    public bool IsSpawn, IsDone;
    private int _dem;
    private static Item1 instance;
    public static Item1 Instance { get => instance; }
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        _dem = 0;
        GetFunc();
    }

    public void GetFunc()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ButtonGameWin.onClick.AddListener(CheckEvent);
        }
        if (!IsSpawn)
        {
            GetItem();
            SpawnItemPopup();
            PlayGame.Instance.IsActiveNC = false;
        }
    }

    private void CheckEvent()
    {
        IsSpawn = false;
    }

    private void GetItem()
    {
        var listItems = from item in GameManager.Instance.ItemSpawn
                        group item by
                        item.GetComponent<Items>().Value;

        foreach (var item in listItems)
        {
            if (_dem >= 2)
                break;
            if (item.Count() >= 3)
            {
                int counter = 0;
                foreach (var item2 in item)
                {
                    if (counter < 3)
                    {
                        StartCoroutine(MoveToItem(item2));
                        GameManager.Instance.ItemSpawn.Remove(item2);
                        counter++;
                    }
                    else
                        break;
                }
            }
            _dem++;
        }
    }

    private void SpawnItemPopup()
    {
        GameObject item = Instantiate(Prefab, transform.position, Quaternion.identity);
        _itemSpawn = item;
        item.transform.Rotate(-90, 0, 0);
        MoveToTarget(item, _targetA);
        IsSpawn = true;
        IsDone = true;
    }

    private void MoveToTarget(GameObject item, GameObject target)
    {
        item.transform.DOMove(target.transform.position, 0.5f);
    }

    private IEnumerator MoveToItem(GameObject item)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 pos = new Vector3(_itemSpawn.transform.position.x,
                                     _itemSpawn.transform.position.y - 20f,
                                     _itemSpawn.transform.position.z);
        item.transform.DOMove(pos, 0.5f);
        item.GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(0.5f);
        MoveToTarget(item, _targetB);
        MoveToTarget(_itemSpawn, _targetB);
        yield return new WaitForSeconds(1f);
        Destroy(item);
        Destroy(_itemSpawn);
    }
}
