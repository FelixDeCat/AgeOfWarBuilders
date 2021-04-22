using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tools.Components
{
    public class PlayerComponent_GroundCheck : MonoBehaviour
    {
        [SerializeField] bool UseFixedUpdate = false;
        [SerializeField] float groundDistance = 0.4f;
        [SerializeField] LayerMask groundMask;

        bool isGrounded;

        public bool IsGrounded { get => isGrounded; }

        private void Update()
        {
            if (!UseFixedUpdate)
            {
                isGrounded = Physics.CheckSphere(this.transform.position, groundDistance, groundMask);
            }
        }
        private void FixedUpdate()
        {
            if (UseFixedUpdate)
            {
                isGrounded = Physics.CheckSphere(this.transform.position, groundDistance, groundMask);
            }
        }
    }
}