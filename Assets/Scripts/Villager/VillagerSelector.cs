using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using UnityEngine.Events;

public class VillagerSelector : MonoBehaviour
{
    [SerializeField] Villager owner;

    bool isOpen = false;

    public void OnEnter()
    {

    }
    public void OnExit()
    {
        UI_Villager_Selector.instance.CloseVillagerOptions();
        isOpen = false;
    }

    public void OnExecute()
    {
        isOpen = !isOpen;
        if (isOpen) UI_Villager_Selector.instance.OpenVillagerOptions(owner);
        else UI_Villager_Selector.instance.CloseVillagerOptions();
    }

}
