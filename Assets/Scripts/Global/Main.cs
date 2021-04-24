using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace AgeOfWarBuilders.Global
{
    public class Main : MonoBehaviour
    {
        public static Main instance;
        private void Awake() => instance = this;

        public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    }
}