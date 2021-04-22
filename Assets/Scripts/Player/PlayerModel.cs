using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Components;

namespace AgeOfWar.Entities
{
    public class PlayerModel : MonoBehaviour
    {
        CharacterController controller;
        public Transform cam;
        public float speed = 12f;
        float move_x;
        float move_z;

        PlayerComponent_GroundCheck groundcheck;

        Vector3 velocity;
        bool isGrounded;
        [SerializeField] float jumpHeight = 3f;
        [SerializeField] float gravity = -9.81f;
        const float GFORCE = -2;

        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            groundcheck = GetComponentInChildren<PlayerComponent_GroundCheck>();
            if (groundcheck == null) throw new System.Exception("No have a [PlayerComponent_GroundCheck], plase add to some child object");
        }

        public void EVENT_MoveHorizontal(float val) => move_x = val;
        public void EVENT_MoveVertical(float val) => move_z = val;

        private void Update()
        {
            isGrounded = groundcheck.IsGrounded;

            if ( isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            Vector3 direction = new Vector3(move_x, 0f, move_z).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0, targetangle, 0) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * GFORCE * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}