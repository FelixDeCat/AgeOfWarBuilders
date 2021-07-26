using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionPlaces { tool, weapon, food, rest, heal }

public class PlaceToAction : MonoBehaviour
{
    [SerializeField] GridComponent gridComponent;

    public ActionPlaces action_type;

    [SerializeField] Animator myAnim;
    [SerializeField] AudioClip clip_execute;

    public bool isInstantOneShot;

    [SerializeField] Transform pos_to_action;
    public Transform Position_to_Action => pos_to_action;

    public void OnExecuted()
    {
        myAnim?.Play("PlaceExecute");
        PlayClip_ExecutePlace();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if(clip_execute) AudioManager.instance.GetSoundPool(clip_execute.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_execute);
        gridComponent.Grid_Initialize(this.gameObject);
        gridComponent.Grid_Rise();
    }
    public void Deinitialize()
    {
        gridComponent.Grid_Deinitialize();
        gridComponent.Grid_Death();
    }

    void PlayClip_ExecutePlace()
    {
        if (clip_execute) AudioManager.instance.PlaySound(clip_execute.name, transform);
    }
}
