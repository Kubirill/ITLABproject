using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class infObjects : MonoBehaviour
{

    public static infObjects _infObjects; //паттерн Singelton
    public List<oneObjectInventory> Items = new List<oneObjectInventory>(); //списк в котором хранятся предметы

    void Awake()
    {
        _infObjects = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    //генерация предмета
    public oneObjectInventory objGet(int win_id)
    {
        oneObjectInventory obj = new oneObjectInventory();
        obj.nameObj = Items[win_id].nameObj;
        obj.image = Items[win_id].image;
        return obj;
    }
}
