using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float speed = 1;
    public Vector3 v3;

    [Header("Ping pong")]
    public bool pingPong;
    public float timeToPong;
    private float _count;
    
    [Header("For Random")]
    public bool random;
    public int random_range;
    float timer;
    public float timetochangerandom = 1;

    public bool constant_movement;

    bool play = true;
    public bool playOnAwake = true;

    private void Awake()
    {
        timer = timetochangerandom + 1;
        play = playOnAwake;
    }

    public void Play() => play = true;
    public void Stop() => play = false;

    void Update ()
    {
        if (!play) return;

        PingPong();
        
        if (random) {
            if (!constant_movement)
            {
                if (timer < timetochangerandom) timer = timer + 1 * Time.deltaTime;
                else
                {
                    v3 = new Vector3(
                        Random.Range(-random_range, random_range),
                        Random.Range(-random_range, random_range),
                        Random.Range(-random_range, random_range));
                    timer = 0;
                }
            }
            else
            {
                v3 = new Vector3(
                           Random.Range(-random_range, random_range),
                           Random.Range(-random_range, random_range),
                           Random.Range(-random_range, random_range));

            }
        }
        transform.Rotate(v3.x * speed, v3.y * speed, v3.z * speed);
    }

    void PingPong()
    {
        if (pingPong)
        {
            _count += Time.deltaTime;

            if (_count >= timeToPong)
            {
                _count = 0;
                v3 = v3 * -1;
            }
        }
    }
}
