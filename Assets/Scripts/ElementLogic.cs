using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementLogic : MonoBehaviour
{
    public int numElement;// уникальный номер элемента, указывающий его правильное местоположение
    public GameObject parent; //родительский объект

 
    public void SetPos()
    {
        transform.localPosition = parent.GetComponent<CubeLogic>().getStart(numElement)-new Vector3(0,0,0.5f);// получить текущие координаты блока и переместиться туда
    }

        private void OnMouseDown()
    {
        
        transform.localPosition = parent.GetComponent<CubeLogic>().Shift(numElement) - new Vector3(0, 0, 0.5f); // вызов функции сдвига у родительского объекта
    }
    
}
