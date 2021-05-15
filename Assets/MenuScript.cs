using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void BUTTON_PLAY()
    {
        Scenes.Load("FirstScene");
    }

    public void BUTTON_EXIT()
    {
        Application.Quit();
    }
}
