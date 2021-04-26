using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GroupNav : MonoBehaviour
{
    public enum Direction {  horizontal , vertical }
    public Direction direction;

    public void ReConnect()
    {
        if (direction == Direction.horizontal)
        {
            this.transform.GetComponentsInChildren<Transform>();
        }
    }
}
