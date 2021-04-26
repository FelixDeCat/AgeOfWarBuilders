using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Selector : MonoBehaviour
{
    [SerializeField] RectTransform myRect;
    static UI_Selector instance;
    void Awake() => instance = this;
    public static void Posicionate(Vector3 pos)
    {
        instance.myRect.gameObject.SetActive(true);
        instance.myRect.position = pos;
    }
    public static void Hide() => instance.myRect.gameObject.SetActive(false);
}
