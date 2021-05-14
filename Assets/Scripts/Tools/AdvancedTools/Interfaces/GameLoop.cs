using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;
using AgeOfWarBuilders.Entities;

public enum PlayableGameType { Initializable, Pausable, Resumable, Updateable }
public class GameLoop : MonoBehaviour
{
    public static GameLoop instance;
    private void Awake() { instance = this; }

    public HashSet<PlayObject> playObjects = new HashSet<PlayObject>();

    public static void AddObject(PlayObject playObject) => instance.Add_PlayObject(playObject);
    public static void RemoveObject(PlayObject playObject) => instance.Remove_PlayObject(playObject);
    public HashSet<PlayObject> PlayObjects => instance.playObjects;
    public static void Pause() => instance.PauseGame();
    public static void Resume() => instance.ResumeGame();
    public static void PlayGame() => instance.StartGame();
    bool inGame;
    bool isPaused;
    bool GameAlreadyInitialized;

    private void Start()
    {
        PlayGame();
    }

    void Add_PlayObject(PlayObject playObject)
    {
        playObject.Index = playObjects.Count;
        playObjects.Add(playObject);
        if(!playObject.IsInitialized) playObject.Initialize();
    }
    void Remove_PlayObject(PlayObject playObject)
    {
        playObjects.Remove(playObject);
        playObject.Deinitialize();
    }

    void StartGame()
    {
        inGame = true;
        foreach(var p in playObjects) { if (!p.IsInitialized) p.Initialize(); }
        GameAlreadyInitialized = true;
    }
    void StopGame()
    {
        inGame = false;
    }
    void PauseGame()
    {
        isPaused = true;
        foreach (var p in playObjects) { p.Pause(); }
    }
    void ResumeGame()
    {
        isPaused = false;
        foreach (var p in playObjects) { p.Resume(); }
    }

    private void Update()
    {
        if (inGame)
        {
            if (PlayerController.DEBUG_PRESS_P)
            {
                isPaused = !isPaused;
            }

            if (!isPaused)
            {
                foreach (var p in playObjects)
                {
                    p.Tick(Time.deltaTime);
                }
            }
        }
    }
}
