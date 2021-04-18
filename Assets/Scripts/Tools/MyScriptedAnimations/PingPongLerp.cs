using UnityEngine;
using System;

public class PingPongLerp
{
    public float timer;

    bool anim;
    bool go;

    float cantspeed = 1f;

    bool loop;

    public Action<float> callback;

    public void Configure(Action<float> _callback, bool _loop)
    {
        callback = _callback;
        loop = _loop;
    }

    public void Play(float _cantspeed)
    {
        timer = 0;
        anim = true;
        go = true;
        cantspeed = _cantspeed;
    }

    public void Stop()
    {
        anim = false;
        timer = 0;
    }

    public void Updatear()
    {
        if (anim)
        {
            if (go)
            {
                if (timer < 1) { timer = timer + cantspeed * Time.deltaTime; callback(timer); }
                else
                {
                    timer = 1;
                    go = false;
                }
            }
            else
            {
                if (timer > 0) { timer = timer - cantspeed * Time.deltaTime; callback(timer); }
                else
                {
                    anim = loop;
                    go = true;
                }
            }
        }
    }
}
