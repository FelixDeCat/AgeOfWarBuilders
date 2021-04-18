using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

public class GoFade_Execute_EndFade : MonoBehaviour
{
    [SerializeField] EventAction ActionCallback = null;
    [SerializeField] UnityEvent EndFade = null;
    public bool noload;

    public void GoFade()
    {
        ActionCallback.Invoke(EndFade.Invoke);
        if (noload)
            EndFade.Invoke();
    }

    void FadeOnFinalized()
    {
        
    }
}
