using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Items : MonoBehaviour
{
    public ItemData ItemData;
    public int Value;
    [SerializeField] private int _currIndex;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Value = ItemData.Value;
        StartCoroutine(AddForce());
    }

    IEnumerator OnMouseDown()
    {
        yield return StartCoroutine(PickItem());
        CheckScale(Value, gameObject);
        Vector3 duration = new Vector3(0, gameObject.transform.localRotation.y, 0);
        gameObject.transform.rotation = Quaternion.Euler(duration);
    }

    IEnumerator PickItem()
    {
        yield return new WaitForSeconds(0.001f);
        if (HandleCtl.Instance.onChoice)
        {
            if (!GameManager.Instance.IsGameOver &&
                GameManager.Instance.Inventory1.Count < 7 &&
                !GameManager.Instance.IsGameWin)
            {
                GameManager.Instance.Pick(gameObject);
            }
        }
    }

    private IEnumerator AddForce()
    {
        yield return new WaitForSeconds(0.3f);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("Plane"))
            {
                _rb.AddForce(Vector3.up * 50f, ForceMode.Impulse);
            }
        }
        yield return new WaitForSeconds(2f);
        _rb.mass = 1000f;
        //_rb.Sleep();    
    }

    private void CheckScale(int valueItem, GameObject item)
    {
        if(valueItem == 6)
        {
            item.transform.localScale = Vector3.one * 30f;
        }
        else if(valueItem == 11)
        {
            item.transform.localScale = Vector3.one * 15f;
        }
        else if (valueItem == 16)
        {
            item.transform.localScale = Vector3.one * 35f;
        }
        else if (valueItem == 12)
        {
            item.transform.localScale = Vector3.one * 35f;
        }
        else if (valueItem == 2)
        {
            item.transform.localScale = Vector3.one * 40f;
        }
        else if (valueItem == 4)
        {
            item.transform.localScale = Vector3.one * 35f;
        }
        else
        {
            if (item.transform.localScale.x >= 50)
            {
                item.transform.localScale = item.transform.localScale;
            }
            else
            {
                item.transform.localScale = Vector3.one * 25f;
            }
        }
    }
}
