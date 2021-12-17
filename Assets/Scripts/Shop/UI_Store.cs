using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Store : MonoBehaviour
{
    [SerializeField] RectTransform parent;
    [SerializeField] UI_BuyItem model;
    [SerializeField] CanvasGroup canvasGroup;

    UI_BuyItem[] items;

    private void Start()
    {
        OpenStore(false);
    }

    public void OpenStore(bool open)
    {
        if (open)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }



    public void CreateStore(Dictionary<int, ItemBuyBehaviour> storedata, Action<int> onBuy)
    {
        items = new UI_BuyItem[storedata.Count];
        foreach (var i in storedata)
        {
            var ui = Instantiate(model, parent);
            var behaviour = i.Value;
            var data = behaviour.myData;

            ui.Configure(onBuy, i.Key, data.name_item, data.description, data.cost.ToString(), data.photo, () => behaviour.CanBuy);
            items[i.Key] = ui;
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Refresh();
        }
    }
}
