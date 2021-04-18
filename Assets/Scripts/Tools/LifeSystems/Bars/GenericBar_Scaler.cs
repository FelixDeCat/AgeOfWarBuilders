using System;
using UnityEngine;

public class GenericBar_Scaler : GenericBar
{
    public Transform pivotPointToScale;
    public enum axis { x, y, z }
    public axis _axis;

    protected override void OnSetValue(float value)
    {
        Vector3 scale = new Vector3(1, 1, 1);
        var percent = (value * 100) / maxValue;
        if (porcentaje) porcentaje.text = !realvalue ? ((int)percent).ToString() + "%" : value + " / " + maxValue;
        if (_axis == axis.x) scale.x = percent * scaler;
        if (_axis == axis.y) scale.y = percent * scaler;
        if (_axis == axis.z) scale.z = percent * scaler;
        
        pivotPointToScale.localScale = scale;
        
    }

    private void Update()
    {
        //Debug.Log(pivotPointToScale.localScale);
    }
}
