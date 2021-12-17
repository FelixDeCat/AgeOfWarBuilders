using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Store", menuName = "Store/BuyItem", order = 1)]
public class BuyItem : ScriptableObject
{
    public string name_item;
    public string description;
    public Sprite photo;
    public int cost;
    public bool buy_unique;
    public ItemBuyBehaviour model_behaviour;
}
