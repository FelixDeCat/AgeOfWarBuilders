using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void BUTTON_PLAY()
    {
        Scenes.Load("Level1");
    }
    public void BUTTON_GOAP()
    {
        Scenes.Load("LevelGOAP");
    }

    public void BUTTON_EXIT()
    {
        Application.Quit();
    }
}
