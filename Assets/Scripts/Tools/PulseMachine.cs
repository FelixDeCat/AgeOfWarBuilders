using UnityEngine;
using System;

public class Pulse
{
    float timer;
    float time_to_pulse;
    bool begin;
    Action OnPulse;

    public Pulse(Action _OnPulse, float _pulse_time = 1)
    {
        OnPulse = _OnPulse;
        time_to_pulse = _pulse_time;
    }

    public void Begin()
    {
        begin = true;
    }
    public void End()
    {
        begin = false;
    }

    public void Tick(float DeltaTime)
    {
        if (begin == false) return;

        if (timer < time_to_pulse)
        {
            timer = timer + 1 * Time.deltaTime;
        }
        else
        {
            timer = 0;
            OnPulse.Invoke();
        }
    }
}
