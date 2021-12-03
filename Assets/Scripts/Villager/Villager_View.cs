using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager_View : MonoBehaviour
{
    [SerializeField] Animator myAnimator;

    [SerializeField] ParticleSystem bloodSplash;

    ///////////////////////////////////////////////////////
    /// ANIMATIONS
    ///////////////////////////////////////////////////////
    
    public void Anim_Walk(bool val)
    {
        myAnimator.SetBool("walk", val);
    }
    public void Anim_Lumberjack(bool val)
    {
        myAnimator.SetBool("lumberjack", val);
    }
    public void Anim_Farm(bool val)
    {
        myAnimator.SetBool("farm", val);
    }

    public void ANIM_SetAttack(bool value)
    {
        myAnimator.SetBool("attack", value);
    }

    public void PLAY_ANIM_Attack()
    {
        myAnimator.Play("hit");
    }

    public void PLAY_ANIM_Walk()
    {
        myAnimator.Play("walk");
    }
    public void PLAY_ANIM_Idle()
    {
        myAnimator.Play("idle");
    }
    public void PLAY_ANIM_Shoot()
    {
        myAnimator.Play("shoot");
    }
    public void PLAY_ANIM_Farm()
    {
        myAnimator.Play("farm");
    }
    public void PLAY_ANIM_Hit()
    {
        myAnimator.Play("hit");
    }

    ///////////////////////////////////////////////////////
    /// PARTICLES
    ///////////////////////////////////////////////////////

    public void Particle_GetPhysicalDamage() { bloodSplash.Play(); }

    ///////////////////////////////////////////////////////
    /// SOUNDS
    ///////////////////////////////////////////////////////
    
    public void Sound_GetPhysicalDamage() { }
}
