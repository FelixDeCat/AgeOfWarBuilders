using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClampedAxisButton
{
    event Action PositiveEvent, NegativeEvent, ReleasePositiveEvent, ReleaseNegativeEvent;

    bool active_positive, active_negative, active_release; //optimization
    bool ispressed = false;
    string axis;
    bool fix;
    bool oneshotRelease;

    enum FromDirection { none ,neg, pos };
    FromDirection fromDirection;

    public ClampedAxisButton(string axis, bool _fix = false) { this.axis = axis; fix = _fix; }
    public void AddEvent_Positive(Action ev_positive) { PositiveEvent += ev_positive; active_positive = true; ispressed = false; }
    public void AddEvent_Negative(Action ev_negative) { NegativeEvent += ev_negative; active_negative = true; ispressed = false; }
    public void AddEvent_Positive_Release(Action ev_release) { ReleasePositiveEvent += ev_release; active_release = true; ispressed = false; }
    public void AddEvent_Negative_Release(Action ev_release) { ReleaseNegativeEvent += ev_release; active_release = true; ispressed = false; }

    public void Refresh()
    {
        if (!fix)
        {
            if (Input.GetAxisRaw(axis) != 0)
            {
                oneshotRelease = false;
                if (Input.GetAxisRaw(axis) == 1)
                {
                    if (!active_positive) return;
                    if (!ispressed)
                    {
                        PositiveEvent.Invoke();
                        ispressed = true;
                        fromDirection = FromDirection.pos;
                    }
                }
                else
                {
                    if (!active_negative) return;
                    if (!ispressed)
                    {
                        NegativeEvent.Invoke();
                        ispressed = true;
                        fromDirection = FromDirection.neg;
                    }
                }
            }
            else
            {
                if (!oneshotRelease)
                {
                    oneshotRelease = true;
                    if (active_release)
                    {
                        if (fromDirection == FromDirection.pos)
                        {
                            ReleasePositiveEvent.Invoke();
                            fromDirection = FromDirection.none;
                        }
                        else if (fromDirection == FromDirection.neg)
                        {
                            ReleaseNegativeEvent.Invoke();
                            fromDirection = FromDirection.none;
                        }
                    }
                }
                ispressed = false;
            }
        }
        else
        {
            if (Mathf.Abs(Input.GetAxis(axis)) >= 0.1f)
            {
                oneshotRelease = false;
                if (Input.GetAxisRaw(axis) > 0.1f)
                {
                    if (!active_positive) return;
                    if (!ispressed)
                    {
                        fromDirection = FromDirection.pos;
                        PositiveEvent.Invoke();
                        ispressed = true;
                    }
                }
                else
                {
                    if (!active_negative) return;
                    if (!ispressed)
                    {
                        fromDirection = FromDirection.neg;
                        NegativeEvent.Invoke();
                        ispressed = true;
                    }
                }
            }
            else
            {
                if (!oneshotRelease)
                {
                    oneshotRelease = true;
                    if (active_release)
                    {
                        if (fromDirection == FromDirection.pos)
                        {
                            ReleasePositiveEvent.Invoke();
                            fromDirection = FromDirection.none;
                        }
                        else if (fromDirection == FromDirection.neg)
                        {
                            ReleaseNegativeEvent.Invoke();
                            fromDirection = FromDirection.none;
                        }
                    }
                }
                ispressed = false;
            }
        }
    }
}
