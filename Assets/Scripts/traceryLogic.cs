using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traceryLogic : MonoBehaviour
{
    public int id;// индификатор объекта
    public int angleSimetry;//угол через которй повторяется узор
    public GameObject cam;// камера
    public bool flip;// сторона объекта

    private void OnMouseDown()
    {
        cam.GetComponent<Inventory>().AddElement(id);// добавить объект в массив
        Destroy(gameObject);//уничтожить
    }
}
