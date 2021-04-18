/////////////////////////////////////////////////////////////
/// Este script funciona en conjunto con el DirectBegin_Scene
/// este script va a estar unicamente en la escena de Carga
/// cuando entra a la escena se va a fijar si ese script existe
/// si es asi, este avisa con un event y hace que se Saltee la
/// escena de Menu
/////////////////////////////////////////////////////////////
namespace Tools.Testing.SceneJumper
{
    using UnityEngine;
    public class DirectBegin_Jumper : MonoBehaviour
    {
        string SceneToJump;
        public static DirectBegin_Jumper instance;
        public void Configure(string s) { SceneToJump = s; DontDestroyOnLoad(this.gameObject); instance = this; }
        public void JumpTo()
        {
            Scenes.Load(SceneToJump);
            Destroy(this.gameObject);
        }
    }
}


