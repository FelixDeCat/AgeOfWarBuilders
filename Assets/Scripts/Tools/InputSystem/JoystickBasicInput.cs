////////////////////////////////////////////////////////
/// JOYSTICK BASIC INPUT By Felix 2019
/// 2019 version:
/// - Basic Buttons Press & Release
/// - Simple return Axis by float
/// 2020 version:
/// -(april) Clamped Axis Buttons to convert Negative and Positive to regular Button
/// -(July) Clamped Axis Buttons suport to Buttons Press & Release
////////////////////////////////////////////////////////

namespace Tools.GameInput
{
    using System;
    using UnityEngine;
    using Tools.ConstNames;

    /*
    A 0
    B 1
    X 2
    Y 3
    Left Bumper 4
    Right Bumper 5
    View (Back) 6
    Menu (Start) 7
    Left Stick Button 8
    Right Stick Button 9
    */

    //ToDo: agregar los booleans que faltan tambien para release, onda de optimizar todavia mas
    //ToDo: agregar casos que faltan, los botones normales no tienen release
    //ToDo: agregar funcion updateada que retorne float para hacer cargas
    public class JoystickBasicInput
    {
        bool[] optimize = new bool[14];

        event Action accept, cancel, square, triangle, lbuttonDown, lbuttonUp, rbuttonDown, rbuttonUp, back, start, rstickCentral, lstickCental = delegate { };
        event Action<float> movehorizontal, movevertical, twisthorizontal, twistvertical = delegate { };


        bool b_accept, b_cancel, b_square, b_triangle,
            b_lbuttonDown, b_lbuttonUp, b_rbuttonDown,
            b_rbuttonUp, b_ltrigger, b_rtrigger, b_dpadLeft,
            b_dpadRight, b_dpadUp, b_dpadDown, b_back, b_start,
            b_movehorizontal, b_movevertical, b_twisthorizontal,
            b_twistvertical, b_rstick_cental, b_lstick_cental;

        ClampedAxisButton DPad_Horizontal = new ClampedAxisButton(ConstUnityJoystickInputNames.AXIS_DPAD_HORIZONTAL);
        ClampedAxisButton DPad_Vertical = new ClampedAxisButton(ConstUnityJoystickInputNames.AXIS_DPAD_VERTICAL);
        ClampedAxisButton Triggers = new ClampedAxisButton(ConstUnityJoystickInputNames.AXIS_TRIGGERS, true);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  PUBLIC BUTTONS
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public JoystickBasicInput SUBSCRIBE_ACCEPT(Action a) { accept += a; b_accept = true; return this; }
        public JoystickBasicInput SUBSCRIBE_CANCEL(Action a) { cancel += a; b_cancel = true; return this; }
        public JoystickBasicInput SUBSCRIBE_SQUARE_X(Action a) { square += a; b_square = true; return this; }
        public JoystickBasicInput SUBSCRIBE_TRIANGLE_Y(Action a) { triangle += a; b_triangle = true; return this; }
        public JoystickBasicInput SUBSCRIBE_LBUTTON_DOWN(Action a) { lbuttonDown += a; b_lbuttonDown = true; return this; }
        public JoystickBasicInput SUBSCRIBE_LBUTTON_UP(Action a) { lbuttonUp += a; b_lbuttonUp = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RBUTTON_DOWN(Action a) { rbuttonDown += a; b_rbuttonDown = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RBUTTON_UP(Action a) { rbuttonUp += a; b_rbuttonUp = true; return this; }
        public JoystickBasicInput SUBSCRIBE_BACK(Action a) { back += a; b_back = true; return this; }
        public JoystickBasicInput SUBSCRIBE_START(Action a) { start += a; b_start = true; return this; }
        public JoystickBasicInput SUBSCRIBE_R_STICK_BTN_CENTRAL(Action a) { rstickCentral += a; b_rstick_cental = true; return this; }
        public JoystickBasicInput SUBSCRIBE_L_STICK_BTN_CENTRAL(Action a) { lstickCental += a; b_lstick_cental = true; return this; }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  PUBLIC DPAD SUBSCRIPTIONS 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // L TRIGGER
        public JoystickBasicInput SUBSCRIBE_PRESS_LTRIGGER(Action a) { Triggers.AddEvent_Negative(a); b_ltrigger = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_LTRIGGER(Action a) { Triggers.AddEvent_Negative_Release(a); b_ltrigger = true; return this; }

        // R TRIGGER
        public JoystickBasicInput SUBSCRIBE_PRESS_RTRIGGER(Action a) { Triggers.AddEvent_Positive(a); b_rtrigger = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_RTRIGGER(Action a) { Triggers.AddEvent_Positive_Release(a); b_rtrigger = true; return this; }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  PUBLIC DPAD SUBSCRIPTIONS 
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // DPAD LEFT
        public JoystickBasicInput SUBSCRIBE_PRESS_DPAD_LEFT(Action a) { DPad_Horizontal.AddEvent_Negative(a); b_dpadLeft = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_DPAD_LEFT(Action a) { DPad_Horizontal.AddEvent_Negative_Release(a); b_dpadLeft = true; return this; }

        // DPAD RIGHT
        public JoystickBasicInput SUBSCRIBE_PRESS_DPAD_RIGHT(Action a) { DPad_Horizontal.AddEvent_Positive(a); b_dpadRight = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_DPAD_RIGHT(Action a) { DPad_Horizontal.AddEvent_Positive_Release(a); b_dpadRight = true; return this; }

        // DPAD UP
        public JoystickBasicInput SUBSCRIBE_PRESS_DPAD_UP(Action a) { DPad_Vertical.AddEvent_Positive(a); b_dpadUp = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_DPAD_UP(Action a) { DPad_Vertical.AddEvent_Positive_Release(a); b_dpadUp = true; return this; }

        // DPAD DOWN
        public JoystickBasicInput SUBSCRIBE_PRESS_DPAD_DOWN(Action a) { DPad_Vertical.AddEvent_Negative(a); b_dpadDown = true; return this; }
        public JoystickBasicInput SUBSCRIBE_RELEASE_DPAD_DOWN(Action a) { DPad_Vertical.AddEvent_Negative_Release(a); b_dpadDown = true; return this; }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  PUBLIC BUTTONS SUBSCRIPTIONS
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public JoystickBasicInput SUBSCRIBE_MOVE_HORIZONTAL(Action<float> a) { movehorizontal += a; b_movehorizontal = true; return this; }
        public JoystickBasicInput SUBSCRIBE_MOVE_VERTICAL(Action<float> a) { movevertical += a; b_movevertical = true; return this; }
        public JoystickBasicInput SUBSCRIBE_TWIST_HORIZONTAL(Action<float> a) { twisthorizontal += a; b_twisthorizontal = true; return this; }
        public JoystickBasicInput SUBSCRIBE_TWIST_VERTICAL(Action<float> a) { twistvertical += a; b_twistvertical = true; return this; }

        public void Refresh()
        {
            if (b_accept) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_A)) accept.Invoke();
            if (b_cancel) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_B)) cancel.Invoke();
            if (b_square) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_Y)) square.Invoke();
            if (b_triangle) if (Input.GetButtonDown(ConstUnityJoystickInputNames.AXIS_DPAD_HORIZONTAL)) triangle.Invoke();

            if (b_lbuttonDown) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_LB)) lbuttonDown.Invoke();
            if (b_lbuttonUp) if (Input.GetButtonUp(ConstUnityJoystickInputNames.BUTTON_LB)) lbuttonUp.Invoke();

            if (b_rbuttonDown) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_RB)) rbuttonDown.Invoke();
            if (b_rbuttonUp) if (Input.GetButtonUp(ConstUnityJoystickInputNames.BUTTON_RB)) rbuttonUp.Invoke();

            if (b_rstick_cental) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_RIGHT_STICKCENTER)) rstickCentral.Invoke();
            if (b_lstick_cental) if (Input.GetButtonUp(ConstUnityJoystickInputNames.BUTTON_LEFT_STICKCENTER)) lstickCental.Invoke();

            if (b_movehorizontal) movehorizontal.Invoke(Input.GetAxis(ConstUnityJoystickInputNames.AXIS_LEFT_HORIZONTAL));
            if (b_movevertical) movevertical.Invoke(Input.GetAxis(ConstUnityJoystickInputNames.AXIS_LEFT_VERTICAL));
            if (b_twisthorizontal) twisthorizontal.Invoke(Input.GetAxis(ConstUnityJoystickInputNames.AXIS_RIGHT_HORIZONTAL));
            if (b_twistvertical) twistvertical.Invoke(Input.GetAxis(ConstUnityJoystickInputNames.AXIS_RIGHT_VERTICAL));

            if (b_dpadLeft || b_dpadRight) DPad_Horizontal.Refresh();
            if (b_dpadDown || b_dpadUp) DPad_Vertical.Refresh();
            if (b_ltrigger || b_rtrigger) Triggers.Refresh();

            if (b_back) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_BACK)) { back.Invoke(); }
            if (b_start) if (Input.GetButtonDown(ConstUnityJoystickInputNames.BUTTON_START)) { start.Invoke(); }
        }
    }
}
