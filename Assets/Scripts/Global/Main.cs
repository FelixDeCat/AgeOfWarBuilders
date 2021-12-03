using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using AgeOfWarBuilders.Entities;


    public class Main : MonoBehaviour
    {
        public static Main instance;
        private void Awake() => instance = this;

    [SerializeField] AudioClip horde;
    [SerializeField] AudioClip preparation;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip lose;
    [SerializeField] AudioClip ambient_day;
    [SerializeField] AudioClip ambient_night;


    private void Start()
    {
        AudioManager.instance.GetSoundPool(horde.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, horde);
        AudioManager.instance.GetSoundPool(preparation.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, preparation);
        AudioManager.instance.GetSoundPool(win.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, win);
        AudioManager.instance.GetSoundPool(lose.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, lose);
        AudioManager.instance.GetSoundPool(ambient_day.name, AudioManager.SoundDimesion.TwoD, AudioGroups.MUSIC, ambient_day);
        AudioManager.instance.GetSoundPool(ambient_night.name, AudioManager.SoundDimesion.TwoD, AudioGroups.MUSIC, ambient_night);

        //PlayClip_AmbientDay();
    }

    public static void PlayClip_Horde() => AudioManager.instance.PlaySound(instance.horde.name);
    public static void PlayClip_Preparation() => AudioManager.instance.PlaySound(instance.preparation.name);
    public static void PlayClip_Win() => AudioManager.instance.PlaySound(instance.win.name);
    public static void PlayClip_Lose() => AudioManager.instance.PlaySound(instance.lose.name);
    public static void PlayClip_AmbientDay() => AudioManager.instance.PlaySound(instance.ambient_day.name);
    public static void PlayClip_AmbientNight() => AudioManager.instance.PlaySound(instance.ambient_night.name);
}
