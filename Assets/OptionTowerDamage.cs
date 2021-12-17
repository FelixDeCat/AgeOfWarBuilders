using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AgeOfWarBuilders.Entities;
using UnityEngine.UI;

public class OptionTowerDamage : MonoBehaviour
{
    Toggle myToogle;
    public bool doDamageTowers;
    public static OptionTowerDamage instance;
    private void Awake()
    {
        instance = this;
        myToogle = GetComponent<Toggle>();
    }

    private void Start()
    {
        myToogle.isOn = false;
        doDamageTowers = false;
    }

    public void ChangeValue(bool newValue)
    {
        doDamageTowers = newValue;
    }
}
