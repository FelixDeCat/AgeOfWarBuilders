using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AgeOfWar.Entities
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] Animator myAnim;
        [SerializeField] PlayerAnimationNames names;

        public void BeginMove()
        {
            myAnim?.SetBool(names.NAME_ON_MOVE, true);
        }
        public void StopMove()
        {
            myAnim?.SetBool(names.NAME_ON_MOVE, false);
        }
    }

    [System.Serializable]
    public class PlayerAnimationNames
    {
        public string NAME_ON_MOVE = "OnMove";
    }
}

