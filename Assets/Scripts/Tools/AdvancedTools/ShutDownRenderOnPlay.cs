
namespace Tools.GameObjectTools
{
    using UnityEngine;

    public class ShutDownRenderOnPlay : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Renderer>().enabled = false;
            enabled = false;
        }
    }
}
