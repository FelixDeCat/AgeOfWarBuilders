using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 3f;


    void Update() {
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");
        
        var dir = new Vector3(x, 0, z).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }

}
