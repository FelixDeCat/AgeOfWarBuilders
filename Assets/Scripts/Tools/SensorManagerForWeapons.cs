using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SensorType { barrido, otros }

//el sensor manager esta pensado para los triggers de las armas
public class SensorManagerForWeapons : MonoBehaviour
{
    [SerializeField] Sensor sensorType = null;
    Sensor current;

    public void SetCallback(Action<GameObject> callback) => sensorType.AddCallback_OnTriggerEnter(callback);

    void InternalEquip(Sensor newsensor)
    {
        if(current) current.Off();
        current = newsensor;
        current.On();
    }

    public void On() => current.On();
    public void Off() => current.Off();

    public void EquipSensor(SensorType sensorType)
    {
        switch (sensorType)
        {
            case SensorType.barrido: InternalEquip(this.sensorType); break;
            case SensorType.otros: break;
        }
    }
}