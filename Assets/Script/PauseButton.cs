using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public GameObject PausePanel;
    public bool IsHome;
    private static PauseButton instance;
    public static PauseButton Instance { get => instance; }
    private void Awake()
    {
        instance = this;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Time.timeScale = 1;
    }

    public void Home()
    {
        Time.timeScale = 1;
        IsHome = true;
        SceneManager.LoadScene(0);
    }
}
