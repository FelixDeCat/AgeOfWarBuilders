using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_NavData
{
    public enum Directions { up = 0, down = 1, left = 2, right = 3 }

    [SerializeField] UI_CustomSelectable[] nav_conections = new UI_CustomSelectable[4];

    public void SetConection(UI_CustomSelectable elem, int direction)
    {
        nav_conections[direction] = elem;
    }

    public UI_CustomSelectable GetElement(int direction)
    {
        return nav_conections[direction];
    }
}
