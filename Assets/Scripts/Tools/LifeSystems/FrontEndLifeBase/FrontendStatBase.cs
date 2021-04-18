using UnityEngine;

public abstract class FrontendStatBase : MonoBehaviour
{
    public abstract void OnValueChange(int value, int max = 100, bool anim = false);
    public abstract void OnValueChangeWithDelay(int value, float delay,int max = 100, bool anim = false);
}
