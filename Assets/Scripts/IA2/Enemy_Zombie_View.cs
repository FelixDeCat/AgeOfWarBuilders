using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zombie_View : MonoBehaviour
{
    [SerializeField] Animator myAnimator;

    [SerializeField] ParticleSystem bloodSplash;

    [SerializeField] AudioClip clip_zombie_takeDamage;
    [SerializeField] AudioClip clip_zombie_rise;
    [SerializeField] AudioClip clip_zombie_die;
    [SerializeField] AudioClip clip_zombie_attack;
    [SerializeField] AudioClip[] clip_zombie_walk;


    ///////////////////////////////////////////////////////
    /// ANIMATIONS
    ///////////////////////////////////////////////////////
    ///
    private void Awake()
    {

        AudioManager.instance.GetSoundPool(clip_zombie_takeDamage.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_zombie_takeDamage);
        AudioManager.instance.GetSoundPool(clip_zombie_rise.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_zombie_rise);
        AudioManager.instance.GetSoundPool(clip_zombie_die.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_zombie_die);
        AudioManager.instance.GetSoundPool(clip_zombie_attack.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_zombie_attack);
        foreach (var c in clip_zombie_walk) AudioManager.instance.GetSoundPool(c.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, c);
    }

    public void Anim_Run(bool run) => myAnimator.SetBool("run", run);
    public void Anim_Attack() { myAnimator.SetTrigger("attack"); Play_Clip_attack(); }
    public void Anim_Death(bool death) { myAnimator.SetBool("death", death); Play_Clip_die(); }
    public void Anim_Rising() { myAnimator.SetTrigger("rising"); Play_Clip_rise(); }
    public void Anim_CombatIdle(bool val) => myAnimator.SetBool("combatIdle", val);

    public void Play_Clip_takeDamage() => AudioManager.instance.PlaySound(clip_zombie_takeDamage.name, transform);//sta
    public void Play_Clip_rise() => AudioManager.instance.PlaySound(clip_zombie_rise.name, transform);//sta
    public void Play_Clip_die() => AudioManager.instance.PlaySound(clip_zombie_die.name, transform);//sta
    public void Play_Clip_attack() => AudioManager.instance.PlaySound(clip_zombie_attack.name, transform);//sta
    public void Play_Clip_Walk() => AudioManager.instance.PlaySound(clip_zombie_walk[Random.Range(0, clip_zombie_walk.Length - 1)].name, transform);

    ///////////////////////////////////////////////////////
    /// PARTICLES
    ///////////////////////////////////////////////////////

    public void Particle_GetPhysicalDamage() { bloodSplash.Play(); }
}
