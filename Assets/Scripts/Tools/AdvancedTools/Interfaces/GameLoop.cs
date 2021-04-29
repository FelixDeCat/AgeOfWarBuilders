using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;

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

    private void Start()
    {
        PlayGame();
    }

    void Add_PlayObject(PlayObject playObject)
    {
        playObject.Index = playObjects.Count;
        playObjects.Add(playObject);
    }
    void Remove_PlayObject(PlayObject playObject)
    {
        playObjects.RemoveAt(playObject.Index);
        playObject.Index = -1;
    }

    void StartGame()
    {
        inGame = true;
        playObjects.ForEach(x => x.Initialize());
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
            if (!isPaused)
            {
                playObjects.ForEach(x => x.Tick(Time.deltaTime));
            }
        }
    }
}
