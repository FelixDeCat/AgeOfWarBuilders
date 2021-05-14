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
        [SerializeField] Transform spawnPoint;
        [SerializeField] Transform mybase;

        private void Start()
        {
            player = FindObjectOfType<PlayerModel>();
        }

        public static Transform SpawnPosition => instance.spawnPoint;
        public MyEventSystem GetMyEventSystem() => MyEventSystem.instance;

        public PlayerModel player;
        public static PlayerModel Player => instance.player;

        public static Vector3 MyBasePosition => instance.mybase.position;
    }
}