using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tools
{
    public class MyEventSystem : MonoBehaviour
    {
        public static MyEventSystem instance;
        EventSystem own;

        [SerializeField] GameObject current;
        StandaloneInputModule inputmodule;

        private void Awake()
        {
            inputmodule = GetComponent<StandaloneInputModule>();
            own = GetComponent<EventSystem>();
            if (instance == null) instance = this;
            else throw new System.Exception("!!!!!!!!!!! Hay dos event system !!!!!!!!!!!!!!");
        }

        private void Update()
        {
            if (own.currentSelectedGameObject != null && own.currentSelectedGameObject != current)
            {
                current = own.currentSelectedGameObject;
            }

            

            //if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            //{
            //    if (own.currentSelectedGameObject == null)
            //    {
            //        own.SetSelectedGameObject(current);
            //    }
            //}
        }
        public EventSystem GetMyEventSystem() => own;

        public void Set_First(GameObject go) { current = go; own.SetSelectedGameObject(go); }
        public void Delete_First() => current = null;
        public void SelectGameObject(GameObject go) { if(go != null) current = go; own.SetSelectedGameObject(go); }
        public void DeselectGameObject() => own.SetSelectedGameObject(null);
    }
}

