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
    public int Value
    {
        get { return val; }
        set
        {

            if (value > 0)
            {
                if (value >= maxVal) //si me quieren agregar mas de lo que tengo permitido
                {
                    if (val >= maxVal)
                    {
                        //si a lo que ya tenia, lo que le quiero agregar supera mi maximo
                        //le digo "No podes agregar mas"
                        CanNotAddMore();
                    }

                    //como supera mi maximo permitido
                    //lo clampeo a mi maximo
                    val = maxVal;
                    OnValueChange(val, maxVal, "el valor es mayor al maximo");
                }
                else
                {

                    if (value < val)
                    {
                        //Si el valor que me quieren pasar es menor
                        //le aviso que "me estan quitando"
                        OnRemove();
                    }
                    else if (value > val)
                    {
                        //Si el valor que me quieren pasar es mayor
                        //le aviso que "me estan agregando"
                        OnAdd();
                    }

                    //como el valor no supera el maximo, ni es menor al minimo
                    //se lo paso directamente
                    val = value;
                    OnValueChange(val, maxVal, "el valor esta entre el rango");
                }
            }
            else
            {
                if (val <= 0)
                {
                    //en el caso que ya tenia 0
                    //quiere decir que quieren seguir quitando
                    //le digo, "ya no podes quitar mas"
                    CanNotRemoveMore();

                }
                else
                {
                    //si tenia al menos un poquito que quiero quitarle ese poquito
                    //directamente le digo "Ya lo perdiste todo"
                    OnLoseAll();
                }


                //sea la manera que sea, lo dejo en 0
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
        Value = maxVal;
    }

    public void IncreaseValue(int val)
    {
        maxVal += val;
        Value = maxVal;
    }

    public void SetValue(int val)
    {
        maxVal = val;
        Value = maxVal;
    }
}
