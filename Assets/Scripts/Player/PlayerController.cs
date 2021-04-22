using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tools.EventClasses;

namespace AgeOfWar.Entities
{
    public class PlayerController : MonoBehaviour
    {
        public EventFloat ev_Horizontal;
        public EventFloat ev_Vertical;

        private void Update()
        {
            ev_Horizontal?.Invoke(Input.GetAxis("Horizontal"));
            ev_Vertical?.Invoke(Input.GetAxis("Vertical"));
        }
    }
}
