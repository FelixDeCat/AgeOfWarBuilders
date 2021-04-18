using System.Collections;
using System.Collections.Generic;
using Tools.Testing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public float timer = 1;
    public string scene = "level_to_change";
    public bool stayHere;

    void Start()
    {
        Invoke("Execute", timer);
    }
    void Execute()
    {
        if (DirectBegin_Jumper.instance != null)
        {
            DirectBegin_Jumper.instance.JumpTo();
            return;
        }
        if (!stayHere)
        {
            Scenes.Load(scene);
        }


    }
    public void ForceExecute()
    {
        SceneManager.LoadScene(scene);
    }
}
