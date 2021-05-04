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

    public List<PlayObject> playObjects = new List<PlayObject>();

    public static void AddObject(PlayObject playObject) => instance.Add_PlayObject(playObject);
    public static void RemoveObject(PlayObject playObject) => instance.Remove_PlayObject(playObject);
    public List<PlayObject> PlayObjects => instance.playObjects;
    public static void Pause() => instance.PauseGame();
    public static void Resume() => instance.ResumeGame();
    public static void PlayGame() => instance.StartGame();
    bool inGame;
    bool isPaused;
    bool AlreadyInitialized;

    private void Start()
    {
        PlayGame();
    }

    void Add_PlayObject(PlayObject playObject)
    {
        playObject.Index = playObjects.Count;
        playObjects.Add(playObject);
        if (!playObject.IsInitialized)
        {
            playObject.Initialize();
        }
    }
    void Remove_PlayObject(PlayObject playObject)
    {
        playObjects.RemoveAt(playObject.Index);
        playObject.Index = -1;
        playObject.Deinitialize();
    }

    void StartGame()
    {
        inGame = true;
        playObjects.ForEach(x => x.Initialize());
        AlreadyInitialized = true;
    }
    void StopGame()
    {
        inGame = false;
    }
    void PauseGame()
    {
        isPaused = true;
        playObjects.ForEach(x => x.Pause());
    }
    void ResumeGame()
    {
        isPaused = false;
        playObjects.ForEach(x => x.Resume());
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
                for (int i = 0; i < PlayObjects.Count; i++)
                {
                    if (playObjects[i] != null)
                        playObjects[i].Tick(Time.deltaTime);
                }

            }
        }
    }
}
