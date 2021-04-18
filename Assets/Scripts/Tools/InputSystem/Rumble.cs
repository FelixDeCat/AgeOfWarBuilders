namespace Tools.GameInput
{
    using UnityEngine;
    //using XInputDotNetPure;

    public class Rumble : MonoBehaviour
    {
        bool rumble = false;
        float time_to_rumble = 0.2f;
        float timer = 0;
        float strenght = 1;
        public void OneShootRumble(float _strengh = 1, float _time_to_rumble = 0.2f)
        {
            time_to_rumble = _time_to_rumble;
            rumble = true;
            strenght = _strengh;
        }

        private void OnApplicationQuit()
        {
            // GamePad.SetVibration(PlayerIndex.One, 0, 0);
        }

        private void OnDisable()
        {
            //  GamePad.SetVibration(PlayerIndex.One, 0, 0);
        }
        public void OnUpdate()
        {
            if (rumble)
            {
                if (timer < time_to_rumble)
                {
                    timer = timer + 1 * Time.deltaTime;
                    // GamePad.SetVibration(PlayerIndex.One, strenght, strenght);
                }
                else
                {
                    rumble = false;
                    timer = 0;
                    //  GamePad.SetVibration(PlayerIndex.One, 0, 0);
                }
            }
        }
    }
}
