using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UI_AnimBase : MonoBehaviour
{
    Action EndOpenAnimation;
    Action EndCloseAnimation;

    public void AddCallbacks(Action EV_End_OpenAnimation, Action EV_End_CloseAnimation)
    {
        EndOpenAnimation = EV_End_OpenAnimation;
        EndCloseAnimation = EV_End_CloseAnimation;
    }

    // para que lo ejecuten mis childs
    protected virtual void ExecuteEndOpenAnimation() => EndOpenAnimation();
    protected virtual void ExecuteEndCloseAnimation() => EndCloseAnimation();

    //para que me llamen desde afuera
    public void Open() => OnOpen();
    public void Close() => OnClose();

    //para avisarles a mis childs que esto sucedió
    protected abstract void OnOpen();
    protected abstract void OnClose();


}
