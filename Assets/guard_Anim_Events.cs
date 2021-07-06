using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class guard_Anim_Events : MonoBehaviour
{
    public UnityEvent OnBeginAttack;
    public UnityEvent OnEndAttack;
    public UnityEvent OnFootStep;

    public void EVENT_Begin_Attack()
    {
        OnBeginAttack.Invoke();
    }
    public void EVENT_End_Attack()
    {
        OnEndAttack.Invoke();
    }

    public void EVENT_FootStep()
    {
        OnFootStep.Invoke();
    }
}
