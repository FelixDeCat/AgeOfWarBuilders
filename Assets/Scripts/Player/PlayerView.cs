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

        public void Play_Slash()
        {
            slash.Stop();
            slash.Play();
        }

        public void BeginMove()
        {
            myAnim?.SetBool(names.NAME_ON_MOVE, true);
        }
        public void StopMove()
        {
            myAnim?.SetBool(names.NAME_ON_MOVE, false);
        }

        public void Run(bool run)
        {
            myAnim?.SetBool(names.NAME_RUN, run);
        }
        public void Jump()
        {
            myAnim?.SetTrigger(names.NAME_JUMP);
        }
        public void IsGrounded(bool isGrounded)
        {
            myAnim?.SetBool(names.NAME_IS_GROUNDED, isGrounded);
        }

        public void Attack()
        {
            myAnim?.SetTrigger(names.NAME_ATTACK);
        }
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

