using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Anim_Code : UI_AnimBase
{
    public bool test_stay_in_my_place;

    CanvasGroup myCanvasGroup;

    [Range(1, 20)]
    public float speed = 9;

    float timer = 0;
    const float time_to_go = 1;
    bool anim;
    bool go;

    Vector3 currentpos;
    Vector3 hidepos;

    public bool usePosition;

    public enum AppearSide { Up, Down, Left, Right }
    public AppearSide side;

    private void Start()
    {
        myCanvasGroup = GetComponentInChildren<CanvasGroup>();
        if (myCanvasGroup) myCanvasGroup.alpha = 0;

        if (usePosition)
        {
            currentpos = transform.localPosition;
            switch (side)
            {
                case AppearSide.Up: hidepos = new Vector3(transform.localPosition.x, transform.localPosition.y + 500, transform.localPosition.z); break;
                case AppearSide.Down: hidepos = new Vector3(transform.localPosition.x, transform.localPosition.y - 500, transform.localPosition.z); break;
                case AppearSide.Left: hidepos = new Vector3(transform.localPosition.x - 500, transform.localPosition.y, transform.localPosition.z); break;
                case AppearSide.Right: hidepos = new Vector3(transform.localPosition.x + 500, transform.localPosition.y, transform.localPosition.z); break;
            }
            if (!test_stay_in_my_place) transform.localPosition = hidepos;
            else transform.localPosition = currentpos;
        }
        else
        {
            
        }
    }

    protected override void OnOpen()
    {
        anim = true;
        go = true;
    }

    protected override void OnClose()
    {
        anim = true;
        go = false;
    }

    public void OnGo(float time_value) 
    {
        if (usePosition) transform.localPosition = Vector3.Lerp(hidepos, currentpos, time_value);
        if(myCanvasGroup) myCanvasGroup.alpha = time_value;
    }
    public void OnBack(float time_value) 
    {
        if (usePosition) transform.localPosition = Vector3.Lerp(currentpos, hidepos, time_value);
        if (myCanvasGroup) myCanvasGroup.alpha = Mathf.Lerp(1,0,time_value);
    }

    private void Update()
    {
        if (anim)
        {
            if (timer < time_to_go)
            {
                timer = timer + speed * Time.deltaTime;

                if (go)
                {
                    OnGo(timer);
                }
                else
                {
                    OnBack(timer);
                }
            }
            else
            {
                if (go) { ExecuteEndOpenAnimation(); }
                else { ExecuteEndCloseAnimation(); }
                timer = 0;
                anim = false;
            }
        }
    }
}
