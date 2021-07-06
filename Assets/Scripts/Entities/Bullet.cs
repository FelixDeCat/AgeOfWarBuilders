using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.Structs;
using System.Linq;
using Tools;
using Tools.Extensions;

public class Bullet : MonoBehaviour
{
    float life_time = 1f;
    float timer = 0f;
    float speed = 50f;
    int damage = 10;
    Action<Bullet, bool> OnDeath;
    bool isAlive;
    string tag_to_damage = "Enemy";

    string floor = "floor";
    Damage damagedata;
    [SerializeField] TrailRenderer myTrail;

    public LayerMask maskExplotion;

    public bool IsABomb;

    public void Configure(Action<Bullet, bool> OnDeath, Vector3 position, Vector3 direction, float life_time = 1f, float speed = 50f, int damage = 10, string tag_to_damage = "Enemy")
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

                if (!IsABomb)
                    Death(false);
                else Explode();
            }
        }

    }

    void Death(bool findtarget)
    {
        OnDeath.Invoke(this, findtarget);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            if (!IsABomb)
            {

                if (other.gameObject.tag == tag_to_damage)
                {
                    other.gameObject.GetComponent<LivingEntity>().ReceiveDamage(damagedata.SetOwnerPosition(this.gameObject.transform.position));
                    Death(true);
                }
            }
            else
            {
                Explode();

                if (other.gameObject.tag == tag_to_damage || other.gameObject.tag == floor)
                {
                    
                }
            }
        }
    }

    void Explode()
    {
        var lives = Physics.OverlapSphere(this.transform.position, 5, maskExplotion)
            .Where(x => x.gameObject.GetComponent<LivingEntity>())
            .Select(X => X.gameObject.GetComponent<LivingEntity>());

        foreach (var l in lives)
        {
            l.ReceiveDamage(damage);
        }

        ParticlesPoolManager.Play_Catapult(this.transform.position);
        Death(true);
    }

    public void On()
    {
        myTrail.Clear();
        myTrail.emitting = true;
        timer = 0;
        isAlive = true;
    }
    public void Off()
    {
        myTrail.emitting = false;
        timer = 0;
        isAlive = false;
    }
}
