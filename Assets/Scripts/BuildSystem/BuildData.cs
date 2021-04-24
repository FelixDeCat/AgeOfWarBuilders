﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "ScriptableObjects/BuildData", order = 1)]
public class BuildData : ScriptableObject
{
    public string nombre;
    public GameObject model;
    public Sprite item_image;
}