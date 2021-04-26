using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfWarBuilders.Entities;
using Tools.UI;
using Tools.Extensions;


namespace AgeOfWarBuilders.BuildSystem
{
    public class BuildManager : MonoBehaviour
    {
        public UI_BuildSelector ui;

        public List<BuildData> ObjectsToBuild;
        public int ObjectCount { get { return ObjectsToBuild.Count; } }
        int indexCursor;

        public Transform InstancePosition;

        #region Apertura y cierre del modo de Contruccion
        [SerializeField] Canvas BuildCanvas;
        [SerializeField] UI_ImageFiller filler;
        float timer;
        float Time_To_TurnOn = 1f;
        bool build_button_pressed = false;
        bool buildModeActive = false;
        bool oneshot;
        void Update_OpenCloseMode()
        {
            if (PlayerController.PRESS_DOWN_BuildMode)
            {
                //no estoy dentro, inicio la cuenta y prendo el filler
                if (!buildModeActive)
                {
                    build_button_pressed = true;
                    filler.TurnOn();
                }
                //ya estoy dentro, con el mismo boton lo apago
                else
                {
                    filler.TurnOff();
                    buildModeActive = false;
                    ExitToBuildMode();
                }

            }
            if (PlayerController.PRESS_Up_BuildMode)
            {
                //no estoy dentro, pero me cancelaron antes, cancelo todo
                if (!buildModeActive)
                {
                    build_button_pressed = false;
                    filler.TurnOff();
                }
                else
                {
                    //nada porque si estoy dentro y suelto la tecla no quiero nada
                }

            }

            if (build_button_pressed)
            {
                if (timer < Time_To_TurnOn)
                {
                    timer = timer + 1 * Time.deltaTime;
                    filler.Fill(timer);
                }
                else
                {
                    filler.TurnOff();
                    timer = 0;
                    build_button_pressed = false;
                    buildModeActive = true;
                    EnterToBuildMode();
                }
            }
            else
            {
                timer = 0;
            }
        }
        void Turn_ON_Canvas() => BuildCanvas.gameObject.SetActive(true);
        void Turn_OFF_Canvas() => BuildCanvas.gameObject.SetActive(false);
        #endregion

        public void NextElement(float direction = 1) // 1:adelante  -1:atras
        {
            indexCursor = direction > 0 ? indexCursor.NextIndex(ObjectCount) : indexCursor.BackIndex(ObjectCount);
            ui.Select(indexCursor);
        }

        private void Start()
        {
            ui.Configurate(() => ObjectsToBuild, OnUIAnimationEndOpen, OnUIAnimationEndClose);
        }

        void OnUIAnimationEndOpen()
        {
            ui.Select(indexCursor);
        }
        void OnUIAnimationEndClose()
        {
            indexCursor = 0;
        }

        void Update()
        {
            Update_OpenCloseMode();
            Update_CursorMovement();
        }

        bool oneshot_axis;
        void Update_CursorMovement()
        {
            if (PlayerController.AXIS__Horizontal_ARROWS != 0)
            {
                if (!oneshot_axis)
                {
                    NextElement(PlayerController.AXIS__Horizontal_ARROWS);
                    oneshot_axis = true;
                }
            }
            else
            {
                oneshot_axis = false;
            }
        }

        void EnterToBuildMode() 
        {
            Turn_ON_Canvas();
            ui.Open();
            
        }
        void ExitToBuildMode() 
        {
            ui.InstantClose();
            Turn_OFF_Canvas();
        }
    }
}

