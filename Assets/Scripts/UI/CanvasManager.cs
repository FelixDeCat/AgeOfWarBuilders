using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    private void Awake() => instance = this;
    [SerializeField] TextMeshProUGUI txt_win_or_lose;
    [SerializeField] GameObject btn_recargar_go;
    [SerializeField] TextMeshProUGUI txt_action_to_do;
    [SerializeField] Button btn_recargar;
    public static void LoseOrWinTextWithAction(string parameter, Action callback, string action_to_do) => instance.LoseOrWinTextWithActionButton(parameter, callback, action_to_do);
    void LoseOrWinTextWithActionButton(string parameter, Action callback, string action_to_do)
    {
        btn_recargar_go.SetActive(true);
        txt_win_or_lose.enabled = true;
        txt_win_or_lose.text = parameter;
        txt_action_to_do.text = action_to_do;
        btn_recargar.onClick.AddListener(callback.Invoke);
    }
}
