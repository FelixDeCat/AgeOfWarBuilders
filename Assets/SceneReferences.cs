using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using AgeOfWarBuilders.Entities;

public class SceneReferences : MonoBehaviour
{
    public static SceneReferences instance;
    private void Awake() => instance = this;

    [Header("Parents Transforms")]
    [SerializeField] Transform parent_MyBuilings;
    [SerializeField] Transform parent_enemies;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform mybase;

    [Header("Principal Objects")]
    [SerializeField] PlayerModel player;

    public static Transform SpawnPosition => instance.spawnPoint;
    public static MyEventSystem MyEventSystem => MyEventSystem.instance;
    
    public static PlayerModel Player => instance.player;
    public static Vector3 MyBasePosition => instance.mybase.position;

    public static Transform parent_MyBuildings => instance.parent_MyBuilings;
    public static Transform parent_Enemies => instance.parent_enemies;
}
