using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tools.UI
{
    public class UI_ImageFiller : MonoBehaviour
    {
        [SerializeField] Image filler;
        [SerializeField] Image aux_image;

        public void Fill(float val)
        {
            filler.fillAmount = val;
        }

        public void TurnOn() { filler.gameObject.SetActive(true); aux_image.gameObject.SetActive(true); }
        public void TurnOff() { filler.gameObject.SetActive(false); aux_image.gameObject.SetActive(false); }

    }
}

