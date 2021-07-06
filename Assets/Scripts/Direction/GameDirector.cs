using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    #region Debugs
    public Text debugState;
    #endregion

    #region Variables
    [SerializeField] float delay_between_states = 0.1f;
    public StateElement[] elements;
    int current = -1;
    float timer_delay = 0;
    bool begin_delay = false;
    Action OnEndGame = delegate { };
    #endregion

    #region MonoBehaviour
    public void Start()
    {
        
        current = -1;
        elements = GetComponentsInChildren<StateElement>();
        if (debugState) debugState.text = current.ToString();
        Invoke("StartGame", 1f);
    }
    void Update()
    {
        Update_TickStates();
        Update_DelayBetweenStates();
    }
    #endregion

    #region Enter and Exit
    public void StartGame()
    {
        NextState();
    }
    public void Add_Callback_OnEndGame(Action cbk) => OnEndGame = cbk;
    void EndGame()
    {
        OnEndGame?.Invoke();
    }
    #endregion

    #region [LOGIC] Index Cursor
    void NextState()
    {
        if(current != -1) elements[current].End();

        current++;
        if (debugState) debugState.text = current.ToString();

        if (current > elements.Length -1)
        {
            current = elements.Length - 1;
            EndGame();
            return;
        }

        elements[current].Initialize(NextState);
        begin_delay = true;
    }
    #endregion

    #region [TICK] Updates
    void Update_DelayBetweenStates()
    {
        if (begin_delay)
        {
            if (timer_delay < delay_between_states)
            {
                timer_delay = timer_delay + 1 * Time.deltaTime;
            }
            else
            {
                elements[current].BeginExecution();
                timer_delay = 0;
                begin_delay = false;
            }
        }
    }
    void Update_TickStates()
    {
        if (current != -1)
        {
            if (current > -1 && current <= elements.Length - 1)
            {
                elements[current].Tick(Time.deltaTime);
            }
        }
    }
    #endregion
}
