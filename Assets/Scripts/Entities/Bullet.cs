using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Structs;

public class Bullet : MonoBehaviour
{
    float life_time = 1f;
    float timer = 0f;
    float speed = 50f;
    int damage = 10;
    Action<Bullet> OnDeath;
    bool isAlive;
    string tag_to_damage = "Enemy";
    Damage damagedata;

    public void Configure(Action<Bullet> OnDeath, Vector3 position, Vector3 direction, float life_time = 1f, float speed = 50f, int damage = 10, string tag_to_damage = "Enemy")
    {
        this.transform.position = position;
        this.transform.forward = direction;
        this.life_time = life_time;
        this.damage = damage;
        this.OnDeath = OnDeath;
        this.tag_to_damage = tag_to_damage;
        this.speed = speed;
        damagedata = new Damage(damage, position, false);
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            this.transform.position += this.transform.forward * speed * Time.deltaTime;

            if (timer < life_time)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                timer = 0;
                Death();
            }
        }
        
    }

    void Death()
    {
        OnDeath.Invoke(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            if (other.gameObject.tag == tag_to_damage)
            {
                other.gameObject.GetComponent<LivingEntity>().ReceiveDamage(damagedata.SetOwnerPosition(this.gameObject.transform.position));
                Death();
            }
        }
    }

    public void On()
    {
        timer = 0;
        isAlive = true;
    }
    public void Off()
    {
        timer = 0;
        isAlive = false;
    }
}
