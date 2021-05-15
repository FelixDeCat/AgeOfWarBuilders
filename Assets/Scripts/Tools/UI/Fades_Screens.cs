using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Fades_Screens : MonoBehaviour
{
    public static Fades_Screens instance;
    [SerializeField] float speed = 0.5f;
    [SerializeField] CanvasGroup canvas_group = null;
    float timer;
    public bool on;
    Action EndOff = delegate { };
    Action EndOn = delegate { };
    Action<string> EndOffParam = delegate { };
    Action<string> EndOnParam = delegate { };
    string parameter;
    public bool Anim=true;
    [SerializeField] bool startOn = false;
    private void Awake() { instance = this; if(startOn) canvas_group.alpha = 1; }
    public void Black() { Anim = false; canvas_group.alpha = 1; }
    public void Transparent() { Anim = false; canvas_group.alpha = 0; }
    public bool IsBlackScreen() => canvas_group.alpha > 0.9f;
    public void FadeOn(Action FadeOnEndCallback)
    {
        EndOn = FadeOnEndCallback;
        timer = 0;
        on = true;
        Anim = true;
    }
    public void FadeOff(Action FadeOffEndCallback)
    {
        EndOff = FadeOffEndCallback;
        timer = 1;
        on = false;
        Anim = true;
    }
    public void FadeOn(Action<string> FadeOnEndCallback, string par)
    {
        EndOnParam = FadeOnEndCallback;
        timer = 0;
        on = true;
        Anim = true;
        parameter = par;
    }
    public void FadeOff(Action<string> FadeOffEndCallback, string par)
    {
        EndOffParam = FadeOffEndCallback;
        timer = 1;
        on = false;
        Anim = true;
        parameter = par;
    }
    void Update()
    {
        if (Anim)
        {
            if (on)
            {
                if (timer < 1)
                {
                    timer = timer + 1 * Time.deltaTime * speed;
                    canvas_group.alpha = timer;
                }
                else
                {
                    timer = 1;
                    Anim = false;
                    EndOn?.Invoke();
                    EndOnParam?.Invoke(parameter);
                }
            }
            else
            {
                if (timer > 0)
                {
                    timer = timer - 1 * Time.deltaTime * speed;
                    canvas_group.alpha = timer;
                }
                else
                {
                    timer = 0;
                    Anim = false;
                    EndOff?.Invoke();
                    EndOffParam?.Invoke(parameter);
                }
            }
        }
    }
}