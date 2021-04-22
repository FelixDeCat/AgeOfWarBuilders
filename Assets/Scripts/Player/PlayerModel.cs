using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfWar.Entities
{
    public class PlayerModel : MonoBehaviour
    {
        Rigidbody rig;
        float speed = 50;

        float move_x;
        float move_z;
        Vector3 moveVertical;
        Vector3 moveHorizontal;
        Vector3 moveResult;

        private void Awake()
        {
            rig = GetComponent<Rigidbody>();
        }

        public void EVENT_MoveHorizontal(float val) => move_x = val;
        public void EVENT_MoveVertical(float val) => move_z = val;

        private void FixedUpdate()
        {
            moveVertical = transform.forward * move_z;
            moveHorizontal = transform.right * move_x;
            moveResult = moveHorizontal + moveVertical;
            moveResult.Normalize();
            rig.velocity = moveResult * speed * Time.fixedDeltaTime;
        }
    }
}