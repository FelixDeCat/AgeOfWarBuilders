using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using AgeOfWarBuilders.Entities;

namespace AgeOfWarBuilders.Global
{
    public class Main : MonoBehaviour
    {
        public static Main instance;
        private void Awake() => instance = this;
    }
}