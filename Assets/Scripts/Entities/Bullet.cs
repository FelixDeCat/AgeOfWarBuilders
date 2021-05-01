using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    bool go;
    public float life_time = 3f;
    float timer;
    public float speed = 50;
    GameObject toIgnore;

    public void Configure( Vector3 direction, GameObject ToIgnore, float life_time = 1f)
    {
        Destroy(this.gameObject, life_time);
        this.transform.forward = direction;
        this.toIgnore = ToIgnore;
    }

    private void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<LivingEntity>().TakeDamage();
            Destroy(this.gameObject);
        }
    }

}
