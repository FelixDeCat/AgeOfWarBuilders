using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CustomInput
{
    Dictionary<GameActions, string> JoystickBindingRoute = new Dictionary<GameActions, string>();
    Dictionary<GameActions, string> KeyboardBindingRoute = new Dictionary<GameActions, string>();

    Dictionary<GameActions, BindingConfig> bindActionConfigurations = new Dictionary<GameActions, BindingConfig>();

    ClampedAxisButton DPad_Horizontal = new ClampedAxisButton(UnityJoystickInputNames.AXIS_DPAD_HORIZONTAL);
    ClampedAxisButton DPad_Vertical = new ClampedAxisButton(UnityJoystickInputNames.AXIS_DPAD_VERTICAL);
    ClampedAxisButton Triggers = new ClampedAxisButton(UnityJoystickInputNames.AXIS_TRIGGERS);

    #region Subscribe to generic Actions
    public CustomInput SubscribeMeTo(GameActions gameaction, BindingConfig bindingconfig)
    {
        if (!bindActionConfigurations.ContainsKey(gameaction))
        {
            bindActionConfigurations.Add(gameaction, bindingconfig);
        }
        return this;
    }
    #endregion

    #region Configurations and Subscriptions
    public CustomInput ConfigureJoystickInput(GameActions gameaction, string buttonname)
    {
        if (!JoystickBindingRoute.ContainsKey(gameaction))
            JoystickBindingRoute.Add(gameaction, buttonname);
        else
        {
            JoystickBindingRoute[gameaction] = buttonname;
        }
        return this;
    }

    
    #endregion

    public void Refresh()
    {
        var aux = new List<GameActions>(bindActionConfigurations.Keys);

        for (int i = 0; i < aux.Count; i++)
        {
            //por ahi tal vez aca pregunto que tipo de input joystick o teclado
            var currentconfig = bindActionConfigurations[aux[i]];

            switch (currentconfig.InputEventAction)
            {
                case InputEventAction.Up: if (Input.GetButtonUp(JoystickBindingRoute[aux[i]])) currentconfig.Execute(); break;
                case InputEventAction.Down: if (Input.GetButtonDown(JoystickBindingRoute[aux[i]])) currentconfig.Execute(); break;
                case InputEventAction.Stay: if (Input.GetButton(JoystickBindingRoute[aux[i]])) currentconfig.Execute(); break;
                case InputEventAction.Axis: currentconfig.Execute(Input.GetAxis(JoystickBindingRoute[aux[i]])); break;
                default: break;
            }
        }
    }
}

public class BindingConfig
{
    InputEventAction inputEventAction;
    public InputEventAction InputEventAction { get => inputEventAction; }
    Action button_action = delegate { };
    Action<float> axis_action = delegate { };
    public BindingConfig(Action _buttonAction, InputEventAction _inputEventAction)
    {
        button_action = _buttonAction;
        inputEventAction = _inputEventAction;
    }
    public BindingConfig(Action<float> _axisAction, InputEventAction _inputEventAction)
    {
        axis_action = _axisAction;
        inputEventAction = _inputEventAction;
    }
    public void Execute() { button_action.Invoke(); }
    public void Execute(float axis) { axis_action.Invoke(axis); }

}