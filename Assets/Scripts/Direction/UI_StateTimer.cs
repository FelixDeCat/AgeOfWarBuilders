using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StateTimer : UI_Base
{
    public TextMeshProUGUI txt_title;
    public TextMeshProUGUI txt_timer;

    public static UI_StateTimer instance;
    private void Awake() => instance = this;

    public static void Refresh(string title, float timer)
    {
        instance.txt_title.text = title;
        if (timer <= 0) timer = 0.0f;
        instance.txt_timer.text = timer.ToString("#0.00");
    }
    public static void Refresh(string title)
    {
        instance.txt_title.text = title;
        instance.txt_timer.text = "";
    }

    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
}
