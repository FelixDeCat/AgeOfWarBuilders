using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBuyBehaviour : MonoBehaviour
{
    public BuyItem myData;
    bool oneshot;

    public void SetDataReference(BuyItem _myData)
    {
        myData = _myData;
    }
    public bool CanBuy
    {
        get
        {
            if (!Store.CanSpendCoins(myData.cost)) return false;
            return myData.buy_unique ? !oneshot : true;
        }
    }

    public void Buy()
    {
        if (myData.buy_unique)
        {
            OnBuy();
            oneshot = true;
            Store.SpendCoin(myData.cost);
        }
        else
        {
            OnBuy();
            Store.SpendCoin(myData.cost);
        }
    }
    protected abstract void OnBuy();
}
