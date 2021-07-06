using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProtection : MonoBehaviour
{
    [SerializeField] int potection_initial_value = 100;
    int protectionlife = 100;
    [SerializeField] GameObject obj;

    [SerializeField] AudioClip clank = null;
    [SerializeField] AudioClip clank_broke = null;
    [SerializeField] ParticleSystem sparks = null;

    private void Awake()
    {
        AudioManager.instance.GetSoundPool(clank.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clank);
        AudioManager.instance.GetSoundPool(clank_broke.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clank_broke);
    }

    public void Initialize()
    {
        obj.SetActive(true);
        protectionlife = potection_initial_value;
    }

    public bool TakeDamage(int damage)
    {
        if (protectionlife > 0)
        {
            protectionlife -= damage;
            if (protectionlife <= 0)
            {
                protectionlife = 0;
                obj.SetActive(false);
                Play_Clank_Broke();
            }
            else
            {
                Play_Clank();
            }
            sparks.Play();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void Play_Clank() => AudioManager.instance.PlaySound(clank.name);
    public void Play_Clank_Broke() => AudioManager.instance.PlaySound(clank_broke.name);


}
