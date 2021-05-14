using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{

    public StateElement[] elements;
    public int current = -1;

    public void Start()
    {
        current = -1;
        elements = GetComponentsInChildren<StateElement>();
        Invoke("StartGame", 1f);
    }

    public void StartGame()
    {
        NextState();
    }

    void NextState()
    {
        if(current != -1) elements[current].End();

        current++;
        if (current > elements.Length -1)
        {
            current = elements.Length - 1;
            EndGame();
            return;
        }

        elements[current].Begin(NextState);
    }

    void EndGame()
    {
        Debug.Log("Endgame");
    }

    public void Update()
    {
        if (current != -1)
        {
            if (current > -1 && current <= elements.Length - 1)
            {
                elements[current].Tick(Time.deltaTime);
            }
        }
    }
}
