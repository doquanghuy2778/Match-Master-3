using UnityEngine;

public class HandleCtl : MonoBehaviour
{
    private static HandleCtl instance;
    public static HandleCtl Instance { get => instance; }
    public LayerMask LayerMask;
    public bool onChoice;
    public bool onClick;
    public AudioClip clip;

    private void Awake()
    {
        HandleCtl.instance = this;
    }

    private void Update()
    {
        InputMouse();
    }

    private void InputMouse()
    {
        #region Pc
        if (Input.GetMouseButtonDown(0))
        {
            onClick = true;
            CheckItem();
        }

        else if (Input.GetMouseButtonUp(0))
            onClick = false;
        #endregion

        #region mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                onClick = true;
        }
        #endregion
    }

    private void CheckItem()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                onChoice = true;
                SoundManager.Instance.PlayClickSound(clip);
            }
            else
                onChoice = false;
        }
    }
}