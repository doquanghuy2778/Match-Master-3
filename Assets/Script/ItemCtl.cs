using System.Collections.Generic;
using UnityEngine;

public class ItemCtl : MonoBehaviour
{
    public int index;
    public float Move_speed;
    private List<GameObject> _items;

    private static ItemCtl instance;
    public static ItemCtl Instance { get => instance; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        index = 0;
        _items = GameManager.Instance.Inventory1;
    }
}
