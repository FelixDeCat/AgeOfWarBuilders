using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HarvestData", menuName = "FarmingSystem/HarvestData", order = 1)]
public class HarvestData : ScriptableObject
{
    public string action_name = "harvesting...";
    public float cd_to_harvest = 5;
    public int elements_per_time = 1;
    public float store_capacity = 1;
}
