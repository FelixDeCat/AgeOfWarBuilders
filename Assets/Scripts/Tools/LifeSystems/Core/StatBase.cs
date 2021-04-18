using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class StatBase
{
    int val;
    int maxVal;
    public int MaxVal => maxVal;
    public int Val
    {
        get { return val; }
        set
        {

            if (value > 0)
            {
                if (value >= maxVal)
                {
                    if (val >= maxVal)
                    {
                        CanNotAddMore();
                    }

                    val = maxVal;
                    OnValueChange(val, maxVal, "el valor es mayor al maximo");
                }
                else
                {
                    if (value < val)
                    {
                        OnRemove();
                    }
                    else if (value > val)
                    {
                        OnAdd();
                    }

                    val = value;
                    OnValueChange(val, maxVal, "el valor esta entre el rango");
                }
            }
            else
            {
                if (val <= 0)
                {
                    CanNotRemoveMore();

                }
                else
                {
                    OnLoseAll();
                }

                val = 0;
                OnValueChange(val, maxVal, "el valor es menor a 0");
            }
        }
    }

    #region Constructor
    public StatBase(int _MaxValue, int _Initial_Value = -1)
    {
        this.maxVal = _MaxValue;
        val = _Initial_Value == -1 ? this.maxVal : _Initial_Value;
        OnValueChange(val, _MaxValue, "Inicializando valor");
    }
    #endregion
    #region Abstracts
    protected abstract void CanNotAddMore();
    protected abstract void OnAdd();
    protected abstract void OnRemove();
    protected abstract void OnLoseAll();
    protected abstract void CanNotRemoveMore();
    protected abstract void OnValueChange(int value, int max, string message);
    #endregion

    /////////////////////////////
    public void ResetValueToMax()
    {
        Val = maxVal;
    }

    public void IncreaseValue(int val)
    {
        maxVal += val;
        Val = maxVal;
    }

    public void SetValue(int val)
    {
        maxVal = val;
        Val = maxVal;
    }
}
