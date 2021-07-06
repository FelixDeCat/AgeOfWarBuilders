using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioGroups { GAME_FX, MUSIC}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _fx = null;
    [SerializeField] private AudioMixerGroup _music = null;

    private Dictionary<string, SoundPool> _soundRegistry = new Dictionary<string, SoundPool>();
    private Dictionary<AudioGroups, AudioMixerGroup> _audioMixers = new Dictionary<AudioGroups, AudioMixerGroup>();

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;

        RegisterAudioMixer();
    }

    private void RegisterAudioMixer()
    {
        _audioMixers.Add(AudioGroups.GAME_FX, _fx);
        _audioMixers.Add(AudioGroups.MUSIC, _music);
    }

    #region SlowMO
    //te puse estas dos funciones aca solo para estructurar... tal vez nisiquiera
    //irian acá... la idea es que cuando entras al Buff o cuando entras a slowMO
    //mande todos los sonidos con el pitch mas grave, a parte de que ejecutaria
    //un sonido de SlowMoEnter, SlowMoLoop y SlowMoExit... tambien haria que el resto
    //de los sonidos tambien tengan este modificador... nunca lo use pero cuando vi que agregaste
    //audiomixergroup supuse que eso lo hacia... te dejo en la carpeta de Sonidos en la carpeta
    //de editados los tres sonidos de slowMO
    public void GoToSlowMo()
    {

    }
    public void BackToSlowMo()
    {

    }
    #endregion

    /// <summary>
    /// Si el soundpool existe, va a reproducir el sonido que llamaron, sino va a tirar un warning
    /// </summary>
    /// <param name="soundPoolName"></param>
    public void PlaySound(string soundPoolName, Transform trackingTransform = null)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            var soundPool = _soundRegistry[soundPoolName];
           
            AudioSource aS = soundPool.Get();
            if (aS == null)
            {
                soundPool.StopAllSounds();
                aS = soundPool.Get();
                if (aS == null)
                    return;
            }

            soundPool.soundPoolPlaying = true;
            if (trackingTransform != null) aS.transform.position = trackingTransform.position;
            aS.Play();

            if (!aS.loop)
                StartCoroutine(ReturnSoundToPool(aS, soundPoolName));
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }
    
    public void PlaySound(string soundPoolName, Action callbackEnd, Transform trackingTransform = null)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            var soundPool = _soundRegistry[soundPoolName];
            
            AudioSource aS = soundPool.Get();
            if (aS == null)
            {
                soundPool.StopAllSounds();
                aS = soundPool.Get();
                if (aS == null)
                    return;
            }
            soundPool.soundPoolPlaying = true;
            if (trackingTransform != null) aS.transform.position = trackingTransform.position;
            aS.Play();

            if (!aS.loop)
                StartCoroutine(ReturnSoundToPool(aS, soundPoolName, callbackEnd));
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }
    public void StopAllSounds(string soundPoolName)
    {
        if (_soundRegistry.ContainsKey(soundPoolName))
        {
            _soundRegistry[soundPoolName].StopAllSounds();
        }
        else
        {
            Debug.LogWarning("No tenes ese sonido en en pool");
        }
    }

    public void PauseSounds()
    {
        foreach (var item in _soundRegistry)
            item.Value.PauseAudio();
    }

    public void ResumeSounds()
    {
        foreach (var item in _soundRegistry)
            item.Value.ResumeAudio();
    }

    /// <summary>
    /// Les devuelve el pool de sonido que pidieron. Si ese pool no existe, crea uno con el audioclip que mandaron
    /// </summary>
    /// <param name="soundPoolName"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    /// 
    public enum SoundDimesion { ThreeD, TwoD }
    public SoundPool GetSoundPool(string soundPoolName, SoundDimesion dimension, AudioGroups audioGroups = AudioGroups.GAME_FX, 
        AudioClip audioClip = null, bool loop = false, int prewarmAmount = 2)
    {
        if (_soundRegistry.ContainsKey(soundPoolName)) return _soundRegistry[soundPoolName];
        else if (audioClip != null) return CreateNewSoundPool(audioClip, soundPoolName, dimension ,audioGroups ,loop, prewarmAmount);
        else return null;
    }

    /// <summary>
    /// Crea el soundpool con el audioclip que manden y lo hace hijo del manager
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="soundPoolName"></param>
    /// <returns></returns>
    private SoundPool CreateNewSoundPool(AudioClip audioClip, string soundPoolName, SoundDimesion dimension, AudioGroups audioGroups = AudioGroups.GAME_FX, 
        bool loop = false, int prewarmAmount = 2)
    {
        var soundPool = new GameObject($"{soundPoolName} soundPool").AddComponent<SoundPool>();
        soundPool.transform.SetParent(transform);
        soundPool.Configure(audioClip, _audioMixers[audioGroups], dimension,loop);
        soundPool.Initialize(prewarmAmount);
        _soundRegistry.Add(soundPoolName, soundPool);
        return soundPool;
    }
    /// <summary>
    /// Borra un soundpool
    /// </summary>
    /// <param name="soundPoolName"></param>
    public void DeleteSoundPool(string soundPoolName)
    {
        Destroy(_soundRegistry[soundPoolName].gameObject);
        _soundRegistry.Remove(soundPoolName);
    }

    /// <summary>
    /// Corutina que devuelve el sonido al pool 
    /// </summary>
    /// <param name="aS"></param>
    /// <param name="sT"></param>
    /// <returns></returns>
    private IEnumerator ReturnSoundToPool(AudioSource aS, string sT, Action EndCallback = null)
    {
        yield return new WaitForSeconds(aS.clip.length);

        EndCallback?.Invoke();
        _soundRegistry[sT].ReturnToPool(aS);
    }
}
