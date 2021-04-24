using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Selector : MonoBehaviour
{

    public RectTransform myRect;

    static UI_Selector instance;
    void Awake()
    {
        instance = this;
    }

    public static void Posicionate(Vector3 pos)
    {
        instance.Pos(pos);
    }

    void Pos(Vector3 pos)
    {
        myRect.position = pos;
    }
}
