using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Structs;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemDataBase", order = 1)]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] Item_Data_Base_Element[] database;
    [SerializeField] Item_Data_Base_Builds_Recipes[] recipes;
}

