using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData
{
    int physical_damage;
    Vector3 owner_position;
    bool hasKnokback;

    public DamageData(int physical_damage, Vector3 owner_position = new Vector3(), bool hasKnockback = false)
    {
        this.physical_damage = physical_damage;
    }
}
