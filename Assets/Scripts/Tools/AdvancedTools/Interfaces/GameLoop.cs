using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Interfaces;
using AgeOfWarBuilders.Entities;
using UnityEngine.UI;
using TMPro;

public enum PlayableGameType { Initializable, Pausable, Resumable, Updateable }
public class GameLoop : MonoBehaviour
{
    #region Variables
    public static GameLoop instance;
    private void Awake() { instance = this; }

    public HashSet<PlayObject> playObjects = new HashSet<PlayObject>();
    
    bool inGame;
    bool isPaused;
    bool GameAlreadyInitialized;

    [SerializeField] TextMeshProUGUI txt_win_or_lose;
    [SerializeField] GameObject btn_recargar_go;
    [SerializeField] Button btn_recargar;

    #endregion

    #region Monovehaviour
    private void Start()
    {
        PlayGame();
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
    #endregion

    #region PlayObjectLogic
    public static void AddObject(PlayObject playObject) => instance.Add_PlayObject(playObject);
    public static void RemoveObject(PlayObject playObject) => instance.Remove_PlayObject(playObject);
    public HashSet<PlayObject> PlayObjects => instance.playObjects;
    void Add_PlayObject(PlayObject playObject)
    {
        playObject.Index = playObjects.Count;
        playObjects.Add(playObject);
        if (!playObject.IsInitialized) playObject.Initialize();
    }
    void Remove_PlayObject(PlayObject playObject)
    {
        playObjects.Remove(playObject);
        playObject.Deinitialize();
    }
    #endregion

    #region GameLoop

    ///////////////////////////////////////////////////
    /// STATICS
    ///////////////////////////////////////////////////
    ///
    public static void Pause() => instance.PauseGame();
    public static void Resume() => instance.ResumeGame();
    public static void PlayGame() => instance.StartGame();
    public static void Lose() => instance.OnLose();
    public static void Win() => instance.OnWin();

    ///////////////////////////////////////////////////
    /// INITIALIZE GAME
    ///////////////////////////////////////////////////
    ///
    void StartGame()
    {
        inGame = true;
        foreach (var p in playObjects) { if (!p.IsInitialized) p.Initialize(); }
        GameAlreadyInitialized = true;
    }
    void StopGame()
    {
        inGame = false;
    }

    ///////////////////////////////////////////////////
    /// PAUSE
    ///////////////////////////////////////////////////
    ///
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

    ///////////////////////////////////////////////////
    /// WIN & LOSE
    ///////////////////////////////////////////////////
    ///
    void OnLose()
    {
        Fades_Screens.instance.FadeOn(EndFade, "Perdiste");
    }
    void OnWin()
    {
        Fades_Screens.instance.FadeOn(EndFade, "Ganaste");
    }

    void EndFade(string param)
    {
        btn_recargar_go.SetActive(true);
        txt_win_or_lose.enabled = true;
        txt_win_or_lose.text = param;
        btn_recargar.onClick.AddListener(ReloadScene);
    }

    void ReloadScene()
    {
        Scenes.ReloadThisScene();
    }
    #endregion


}
