using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem_LifeTowers : ItemBuyBehaviour
{
    public bool isActive;
    public int cantToAdd = 200;
    public static BuyItem_LifeTowers instance;
    private void Awake()
    {
        instance = this;
    }

    protected override void OnBuy()
    {
        var towers = FindObjectsOfType<TowerEntity>();
        foreach (var t in towers)
        {
            t.TryToAddLife();
        }
    }
}
