using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_BuyItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image myImage;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] Button btn_interact;

    int myId;
    Action<int> OnPress;
    Func<bool> canBuy;

    public void Configure(Action<int> _OnPress, int _id, string _myName, string _description, string _cost, Sprite _myImage, Func<bool> _canBuy)
    {
        btn_interact.onClick.AddListener(PRESS_PressBuy);
        OnPress = _OnPress;
        myId = _id;
        myName.text = _myName;
        description.text = _description;
        myImage.sprite = _myImage;
        cost.text = _cost;
        canBuy = _canBuy;
        Refresh();
    }

    public void Refresh()
    {
        btn_interact.interactable = canBuy.Invoke();
        cost.color = canBuy.Invoke() ? Color.green : Color.red;
    }

    public void PRESS_PressBuy()
    {
        if (canBuy.Invoke())
        {
            OnPress.Invoke(myId);
        }

        Refresh();
    }
}
