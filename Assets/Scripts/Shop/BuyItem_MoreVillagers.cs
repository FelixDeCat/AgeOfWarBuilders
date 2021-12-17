using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem_MoreVillagers : ItemBuyBehaviour
{
    public VillagerProfesion profession;

    protected override void OnBuy()
    {
        SpawnVillager.SpawnNewVillager(profession);
    }
}
