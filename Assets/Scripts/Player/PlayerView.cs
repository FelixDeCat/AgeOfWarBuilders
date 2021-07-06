using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfWarBuilders.Entities
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] Animator myAnim;
        [SerializeField] PlayerAnimationNames names;

        [SerializeField] ParticleSystem slash;
        [SerializeField] ParticleSystem hit;

        [SerializeField] AudioClip clip_sword_Whoosh;
        [SerializeField] AudioClip clip_TakeDamage;
        [SerializeField] AudioClip clip_Die;
        [SerializeField] AudioClip[] clips_Walk;


        private void Awake()
        {
            AudioManager.instance.GetSoundPool(clip_sword_Whoosh.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clip_sword_Whoosh);
            AudioManager.instance.GetSoundPool(clip_TakeDamage.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clip_TakeDamage);
            AudioManager.instance.GetSoundPool(clip_Die.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, clip_Die);
            foreach (var c in clips_Walk) AudioManager.instance.GetSoundPool(c.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, c);
        }

        public void PLay_Hit() { hit.Stop(); hit.Play(); }
        public void Play_Slash() { slash.Stop(); slash.Play(); Play_Clip_SwordWhoosh(); }
        public void BeginMove() => myAnim?.SetBool(names.NAME_ON_MOVE, true);
        public void StopMove() => myAnim?.SetBool(names.NAME_ON_MOVE, false);
        public void Run(bool run) => myAnim?.SetBool(names.NAME_RUN, run);
        public void Jump() => myAnim?.SetTrigger(names.NAME_JUMP);
        public void IsGrounded(bool isGrounded) => myAnim?.SetBool(names.NAME_IS_GROUNDED, isGrounded);
        public void Attack() => myAnim?.SetTrigger(names.NAME_ATTACK);

        public void Play_Clip_SwordWhoosh() => AudioManager.instance.PlaySound(clip_sword_Whoosh.name);
        public void Play_Clip_TakeDamage() => AudioManager.instance.PlaySound(clip_TakeDamage.name);
        public void Play_Clip_Die() => AudioManager.instance.PlaySound(clip_Die.name);
        public void Play_Clip_Walk() => AudioManager.instance.PlaySound(clips_Walk[Random.Range(0, clips_Walk.Length-1)].name);
    }

    [System.Serializable]
    public class PlayerAnimationNames
    {
        public string NAME_ON_MOVE = "OnMove";
        public string NAME_RUN = "run";
        public string NAME_JUMP = "jump";
        public string NAME_IS_GROUNDED = "isGrounded";
        public string NAME_ATTACK = "Attack";
    }
}

