using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public int CurrLevel;
    private static SaveGame instance;
    public static SaveGame Instance { get => instance; }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CurrLevel = 1;
    }
}
