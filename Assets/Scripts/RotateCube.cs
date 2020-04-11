using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public float maxDistance;//максимальная дистанция от камеры
    public float minDistance;//минимальная дистанция от камеры
    public float speedWheel;//скорость изменения дистанции
    public float speedRotate = 1f;//скорость вращения
    public Transform targetLook;//объект свращения
    public Transform cameraPos;//трансформ камерры
    public Transform cameraVector;//вспомогательнлый трансформ для получения вектора направления камеры
    public float startX = 0;//начальные координаты
    public float startY = 0;//
    public bool active = false;//переменная активируется при смене объекта вращения

    private float nowDistance;//текущее расстояние от камеры
    private Vector3 axisOne;//вектора вращения объекта
    private Vector3 axisTwo;
    private Vector2 startMouse;//начальные координаты мыши

    // Start is called before the first frame update
    void Start()
    {
        nowDistance = (maxDistance + minDistance ) / 2;// установить текущие расстояние мжду максимальным и минимальным
        targetLook.position = new Vector3(targetLook.localPosition.x, targetLook.localPosition.y, nowDistance);// установить объект вращения на исходную позицию
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)// пока объект не меняется
        {
            nowDistance = nowDistance + Input.GetAxis("Mouse ScrollWheel") * speedWheel;// изменить дистанцию под действием колёсика мыши
            nowDistance = Mathf.Min(Mathf.Max(minDistance, nowDistance), maxDistance);// держать расстояние в диапозоне
            targetLook.localPosition = new Vector3(targetLook.localPosition.x, targetLook.localPosition.y, nowDistance);// изменит положение объекта с учётом новой координаты

        }
        else // если объект вращения новый
        {
            if ((targetLook.localPosition != new Vector3(startX, startY, nowDistance)))// пока объект не пришёл в исходную точку...
            {
                targetLook.localPosition = Vector3.Lerp(targetLook.localPosition, new Vector3(startX, startY, nowDistance), 0.05f);//двигать объек к ней
                targetLook.localScale = Vector3.Lerp(targetLook.localScale, new Vector3(1, 1, 1), 0.05f);// и изменять размер
            }
            if (Vector3.Distance(targetLook.localPosition, new Vector3(startX, startY, nowDistance))<1)// если объект почти у исходной точки..
            {
                targetLook.localPosition = new Vector3(startX, startY, nowDistance);//то переместить к ней
                active = false;// и закончить перемещение

            }
        }
    }
    

    public void OnMouseDown()//при нажатие мыши
    {
        startMouse = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);//получить координаты мыши
        axisOne = new Vector3(cameraVector.position.x - cameraPos.position.x, cameraVector.position.y - cameraPos.position.y, cameraVector.position.z - cameraPos.position.z);//получить вектор исходящий из камеры
        axisTwo = axisOne;//аналогично второму вектору
        axisOne = new Vector3(axisOne.z, axisOne.y, -axisOne.x);// с помощью ЛИНАЛА находим новый вектор, вокруг которого будет вращаться объект
        axisTwo = new Vector3( axisOne.y, -axisOne.x, axisOne.z);// с помощью ЛИНАЛА находим новый другой вектор, вокруг которого будет вращаться объект
    }
    public void OnMouseUp()
    {
        startMouse = new Vector2 (0f,0f);// если мышь отпустили, то обнулить координаты
    }
    public void OnMouseDrag()
    {
        Vector2 nowMouse;//переменная с текущими координатами мыши
        
        
            nowMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//получить координаты мыши
            if (startMouse != nowMouse)// если стартовые координаты и текущие не равны
            {
                // targetLook.SetPositionAndRotation(targetLook.position, Quaternion.Euler(startX - Input.mousePosition.y, startY - Input.mousePosition.x, targetLook.rotation.z));
                //targetLook.RotateAround();
                targetLook.RotateAround(targetLook.transform.position, -axisTwo, (startMouse.x - nowMouse.x) * speedRotate);//вращать вокруг координат вращения, в зависимости от разницы текущего и стартового положения
                targetLook.RotateAround(targetLook.transform.position, -axisOne, (startMouse.y - nowMouse.y) * speedRotate);
                startMouse = nowMouse;//стартовое положение становится равно текущему
            }
            
        
    }
     
    public void newTarget(Transform target)//процедура присвоение нвой цели для слежения
    {
        minDistance = 2;
        targetLook = target;
       // transform.LookAt(targetLook);
        targetLook.parent=transform;
        nowDistance = (maxDistance + minDistance) / 2;
        active = true;
    }
}
