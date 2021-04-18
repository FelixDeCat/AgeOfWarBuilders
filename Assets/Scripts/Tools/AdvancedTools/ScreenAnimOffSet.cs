using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenAnimOffSet : MonoBehaviour {

    RawImage raw;

    public Rect r;

    private void Awake()
    {
        raw = gameObject.GetComponent<RawImage>();
    }

    void Start () {
        r = raw.uvRect;
	}
	
	void Update () {
        r.x += 0.003f * Time.deltaTime;
        r.y += 0.002f * Time.deltaTime;
        raw.uvRect = r;
	}
}
