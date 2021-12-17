using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVillager : MonoBehaviour
{
    public static SpawnVillager instance;
    public Villager model;
    public Transform spawnPos;

    private void Awake()
    {
        instance = this;
    }

    public static void SpawnNewVillager(VillagerProfesion profession) => instance.SpawnNewVillagerInPos(profession);

    void SpawnNewVillagerInPos(VillagerProfesion profession)
    {
        var villager = GameObject.Instantiate(model);
        villager.transform.position = spawnPos.position;
        villager.ConfigureProfession(profession);
    }
}
