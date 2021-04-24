using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

namespace AgeOfWar.Entities
{
    public class PlayerController : MonoBehaviour
    {
        ////////////////////////////////////////////////////////////////////////
        ////  
        ////////////////////////////////////////////////////////////////////////
        private static PlayerController instance;
        public static PlayerController Instance { get => instance; }
        private void Awake() => instance = this;

        ////////////////////////////////////////////////////////////////////////
        ////  PUBLIC STATICS
        ////////////////////////////////////////////////////////////////////////
        public static float AXIS_Horizontal     => instance.AxisHorizontal(); 
        public static float AXIS_Vertical       => instance.AxisVertical();
        public static bool PRESS_DOWN_Jump      => instance.ButtonDownJump();
        public static bool PRESS_UP_Jump        => instance.ButtonUpJump();
        public static bool HOLD_Jump            => instance.ButtonJump();
        public static bool PRESS_DOWN_Skill_1   => instance.ButtonDownSkill1();
        public static bool PRESS_DOWN_Skill_2   => instance.ButtonDownSkill2();
        public static bool PRESS_DOWN_Skill_3   => instance.ButtonDownSkill3();

        ////////////////////////////////////////////////////////////////////////
        ////  PRIVATES
        ////////////////////////////////////////////////////////////////////////
        float AxisHorizontal()  { return Input.GetAxis("Horizontal"); }
        float AxisVertical()    { return Input.GetAxis("Vertical"); }
        bool ButtonDownJump()   { return Input.GetButtonDown("Jump"); }
        bool ButtonUpJump()     { return Input.GetButtonUp("Jump"); }
        bool ButtonJump()       { return Input.GetButton("Jump"); }
        bool ButtonDownSkill1() { return Input.GetButtonDown("Skill1"); }
        bool ButtonDownSkill2() { return Input.GetButtonDown("Skill2"); }
        bool ButtonDownSkill3() { return Input.GetButtonDown("Skill2"); }
    }

}
