using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRaw : MonoBehaviour
{

    public float scrollSpeed;
    public RawImage m_material;
    // Update is called once per frame
    public float max_Verical_Ping_pong = 0.15f;

    PingPongLerp pongLerp;

    private void Start()
    {
        pongLerp = new PingPongLerp();
        pongLerp.Configure(Anim, true);
        pongLerp.Play(1);
    }

    void Anim(float anim_val)
    {
        m_material.uvRect = new Rect(Mathf.Lerp(-max_Verical_Ping_pong, max_Verical_Ping_pong, anim_val), 0, 1, 1);
    }

    void Update()
    {
        pongLerp.Updatear();
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1); 
        m_material.uvRect = new Rect(x, x, 1, 1);
    }
}
