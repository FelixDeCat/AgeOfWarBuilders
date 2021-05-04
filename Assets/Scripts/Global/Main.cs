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
        [SerializeField] Transform spawnPoint;

        public static Transform SpawnPosition => instance.spawnPoint;
        public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;
    }
}