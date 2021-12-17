using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem_FastTower : ItemBuyBehaviour
{
    public static BuyItem_FastTower instance;
    private void Awake()
    {
        instance = this;
    }
    public bool isActive;
    public float global_speed_modifier;
    protected override void OnBuy()
    {
        global_speed_modifier = 2;
        isActive = true;
    }
}
