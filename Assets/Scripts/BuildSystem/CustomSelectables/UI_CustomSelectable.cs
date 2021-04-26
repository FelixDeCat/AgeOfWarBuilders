using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_CustomSelectable : MonoBehaviour
{
    RectTransform myRect;
    //public UI_NavData navigation;
    [SerializeField] Image img;
    void Awake()
    {
        myRect = GetComponent<RectTransform>();

    }
    public void Print()
    {
        Debug.Log("myRect.anchoredPosition" + myRect.anchoredPosition);
        Debug.Log("myRect.anchoredPosition3D" + myRect.anchoredPosition3D);
        Debug.Log("MyRect.forward" + myRect.forward);
        Debug.Log("MyRect.gameObject.transform.position" + myRect.gameObject.transform.position);
        Debug.Log("MyRect.gameObject.transform.localPosition" + myRect.gameObject.transform.localPosition);
        Debug.Log("MyRect.pivot" + myRect.pivot);
        Debug.Log("MyRect.position" + myRect.position);
        Debug.Log("MyRect.rect.position" + myRect.rect.position);
        Debug.Log("MyRect.rect.x" + myRect.rect.x);
        Debug.Log("MyRect.rect.y" + myRect.rect.y);
    }

    public void SetImage(Sprite sprite)
    {
        img.sprite = sprite;
    }

    public void Select()
    {
        UI_Selector.Posicionate(myRect.position);
        OnSelect();
    }
    protected abstract void OnSelect();
}
