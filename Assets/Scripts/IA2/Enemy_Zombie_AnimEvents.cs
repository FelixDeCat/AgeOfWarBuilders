using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_Zombie_AnimEvents : MonoBehaviour
{
    [SerializeField] UnityEvent OnAnimAttack;
    [SerializeField] UnityEvent OnAnimDeathEnded;

    public void ANIM_EVENT_Attack()
    {
        OnAnimAttack?.Invoke();
    }
    public void ANIM_EVENT_Death()
    {
        OnAnimDeathEnded?.Invoke();
    }
}
