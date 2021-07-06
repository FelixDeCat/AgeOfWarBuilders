using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Pool;
using System;

public class Bomb_PoolManager : MonoBehaviour
{
    public static Bomb_PoolManager instance;
    GenericPoolManager<Bullet> pool;
    [SerializeField] Bullet bullet_model;
    [SerializeField] Transform parent;
    [SerializeField] AudioClip bomb_collision;

    readonly Vector3 FAR = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);

    private void Awake()
    {
        instance = this;
        pool = new GenericPoolManager<Bullet>(Create, TurnOn, TurnOff, "BombBasic", true, 10);
        AudioManager.instance.GetSoundPool(bomb_collision.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, bomb_collision);
    }

    public void Shoot(Vector3 position, Vector3 direction, float life_time = 1f, float speed = 50f, int damage = 10, string tag_to_damage = "Enemy")
    {
        var bullet = pool.Get();
        bullet.Configure(OnBulletDeath, position, direction, life_time, speed, damage, tag_to_damage);
        bullet.On();
    }

    void OnBulletDeath(Bullet bullet, bool findtarget)
    {
        bullet.Off();
        pool.Return(bullet);
        if(findtarget) AudioManager.instance.PlaySound(bomb_collision.name, bullet.transform);
    }

    void TurnOn(Bullet obj) => obj.gameObject.SetActive(true);
    void TurnOff(Bullet obj) => obj.gameObject.SetActive(false);

    Bullet Create(object obj)
    {
        return Instantiate(bullet_model, parent.transform.position, parent.transform.rotation, parent);
    }

}
