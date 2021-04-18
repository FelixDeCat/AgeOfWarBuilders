using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    public void Awake()
    {
        //var myGameCores = FindObjectsOfType<DontDestroy>().Where(x => x != this).ToArray();

        //for (int i = 0; i < myGameCores.Length; i++)
        //    Destroy(myGameCores[i].gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
