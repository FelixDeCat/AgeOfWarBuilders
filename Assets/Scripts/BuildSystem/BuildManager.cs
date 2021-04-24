using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AgeOfWarBuilders.Entities;
using Tools.UI;

namespace AgeOfWarBuilders.BuildSystem
{
    public class BuildManager : MonoBehaviour
    {
        public UI_BuildSelector ui;

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
        void OnCanvas() => BuildCanvas.gameObject.SetActive(true);
        void OffCanvas() => BuildCanvas.gameObject.SetActive(false);
        #endregion

        void Update()
        {
            Update_OpenCloseMode();
        }


        void EnterToBuildMode() 
        {
            ui.Open();
            OnCanvas();
        }
        void ExitToBuildMode() 
        {
            ui.InstantClose();
            OffCanvas();
        }
    }
}

