
namespace Tools.Screen
{
    using UnityEngine;

    public static class ScreenLimits
    {
        public static Vector2 Right_Superior { get { return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)); } }
        public static Vector2 Left_Inferior { get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)); } }
    }
}

