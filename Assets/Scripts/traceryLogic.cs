using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traceryLogic : MonoBehaviour
{
    public int id;
    public int angleSimetry;
    public GameObject cam;
    public bool flip;

    private void OnMouseDown()
    {
        cam.GetComponent<Inventory>().AddElement(id);
        Destroy(gameObject);
    }
}
