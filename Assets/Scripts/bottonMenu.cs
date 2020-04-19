using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bottonMenu : MonoBehaviour
{
    public bool contin; //если true, то это кнопка продолжить
    public float speed=1;//скорость движения кнопки
    private int direction=0;//направление движеия кнопки

    private void OnMouseDown()
    {
        direction = 1;//при нажатие на кнопку меняет движение кнопки по направлению внутрь
    }

    private void Update()
    {
        transform.position = transform.position + new Vector3(0, 0, speed * direction*Time.deltaTime);// менять позицию кнопки
        if (transform.position.z > -4.9) direction = -1;//если позиция кнопки дошла до "нажатого" состояния, то изменить направление
        
        if ((transform.position.z < -5) && (direction == -1) && (contin))//если кнопка движется, она вернулась в исходное состояние и это кнопка "продолжить"
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");//найти камеру
            SceneManager.LoadScene("Level"+(cam.GetComponent<Menu>().GetLastLevel()), LoadSceneMode.Single);// перейти на последний игранный уровень
        }
        if ((transform.position.z < -5) && (direction == -1) && (!contin))//если кнопка движется, она вернулась в исходное состояние и это кнопка не "продолжить"
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");//найти камеру
            SceneManager.LoadScene(gameObject.name, LoadSceneMode.Single);//
            cam.GetComponent<Menu>().Save(1);//Перключить на первую стадию
            cam.GetComponent<Menu>().NewTime(0);//Обнулить время
        }
    }
}
