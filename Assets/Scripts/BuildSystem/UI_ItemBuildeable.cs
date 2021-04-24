using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemBuildeable : Selectable
{
    public Image photo;
    RectTransform myRect;

    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log("ON SELECT");
        base.OnSelect(eventData);

        UI_Selector.Posicionate(myRect.position);
    }

}
