using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestContainer : MonoBehaviour
{
    public Container myInventory;

    public ElementData element_to_add;
    public ElementData element_to_add_2;
    public int ID_to_Add;

    private void Start()
    {
        myInventory = new Container(10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            myInventory.AddElement(element_to_add, 12);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            myInventory.AddElement(element_to_add_2, 3);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {

        }
        if (Input.GetKeyDown(KeyCode.N))
        {

        }
        if (Input.GetKeyDown(KeyCode.M))
        {

        }
    }
}
