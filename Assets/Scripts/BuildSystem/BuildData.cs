using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Structs;

[CreateAssetMenu(fileName = "BuildData", menuName = "ScriptableObjects/BuildData", order = 1)]
public class BuildData : ScriptableObject
{
    public string nombre;
    public TowerEntity model;
    public GameObject[] build_phases; 
    public GameObject model_BuildMode;
    public Sprite item_image;
    public Item_Recipe[] requirements;
}
