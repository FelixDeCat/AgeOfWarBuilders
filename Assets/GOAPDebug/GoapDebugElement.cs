using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoapDebugElement : MonoBehaviour
{
    string my_name;
    [SerializeField] TextMeshProUGUI txt_Action;
    [SerializeField] TextMeshProUGUI txt_value;
    Action<string> CallbackPressChange;
    public void Configure(Action<string> cbk_change, string s)
    {
        CallbackPressChange = cbk_change;
        txt_Action.text = s;
        my_name = s;
    }
    public void SetValue(object obj)
    {
        txt_value.text = obj.ToString();
    }
    public void SetActiveAction(bool v)
    {
        txt_Action.color = v ? Color.green : Color.red;
    }
    public void PRESS_BUTTON_DOWN()
    {
        CallbackPressChange.Invoke(my_name);
    }
}
