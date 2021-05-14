using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zombie_View : MonoBehaviour
{
    [SerializeField] Animator myAnimator;

    [SerializeField] ParticleSystem bloodSplash;

    ///////////////////////////////////////////////////////
    /// ANIMATIONS
    ///////////////////////////////////////////////////////
    
    public void Anim_Run(bool run)
    {
        myAnimator.SetBool("run", run);
    }
    public void Anim_Attack()
    {
        myAnimator.SetTrigger("attack");
    }
    public void Anim_Death(bool death)
    {
        myAnimator.SetBool("death", death);
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
