using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerTool : MonoBehaviour
{
    public GameObject axe;
    public GameObject pickaxe;
    public GameObject trident;


    public void ReconfigureTool(VillagerProfesion prof)
    {
        OffAll();
        switch (prof)
        {
            case VillagerProfesion.miner:       pickaxe.SetActive(true);    break;
            case VillagerProfesion.farmer:      trident.SetActive(true);    break;
            case VillagerProfesion.lumberjack:  axe.SetActive(true);        break;
            case VillagerProfesion.warrior:                                 break;
        }
    }

    void OffAll() { pickaxe.SetActive(false); axe.SetActive(false); trident.SetActive(false); }
}