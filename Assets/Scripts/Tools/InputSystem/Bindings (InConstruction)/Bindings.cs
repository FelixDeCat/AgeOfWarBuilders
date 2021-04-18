namespace Tools.GameInput.deprecated
{
    using UnityEngine;
    using Tools.ConstNames;

    public class Bindings : MonoBehaviour
    {
        CustomInput customInput;

        private void Start()
        {
            customInput = new CustomInput();
            customInput.ConfigureJoystickInput(GameActions.Right_Hand, ConstUnityJoystickInputNames.BUTTON_A);
            customInput.SubscribeMeTo(GameActions.Right_Hand, new BindingConfig(Attack, InputEventAction.Down));
        }

        private void Update()
        {
            //ejemplo de como se cambian los inputs

            if (Input.GetKeyDown(KeyCode.H))
            {
                customInput.ConfigureJoystickInput(GameActions.Right_Hand, ConstUnityJoystickInputNames.BUTTON_A);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                customInput.ConfigureJoystickInput(GameActions.Right_Hand, ConstUnityJoystickInputNames.BUTTON_B);
            }

            customInput.Refresh();
        }

        void Attack()
        {
            Debug.Log("ATTACK");
        }
    }
}