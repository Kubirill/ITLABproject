using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    float fpsTime = 0;//количесво кадров 
    private int nowTime;//время
    public GameObject cam;//объект камеры

    private void Awake()
    {
        nowTime = cam.GetComponent<Menu>().GetTime();//загрузить сохранённое время
        gameObject.GetComponent<Text>().text = Zeropad(nowTime / 60, 2) + ":" + Zeropad(nowTime % 60, 2);//пишем тест с помощью функции добавления 0
    }

    private void Update()
    {
        fpsTime = fpsTime + Time.deltaTime;//добавляем  долю секунд
        if (fpsTime >= 1)//если прошла секунда
        {
            fpsTime = fpsTime - 1;// то отнимаем секунду от долей
            nowTime++;cam.GetComponent<Menu>().NewTime(nowTime);//добавляем секунду к текущему времени
            gameObject.GetComponent<Text>().text = Zeropad(nowTime / 60, 2) + ":" + Zeropad(nowTime % 60, 2);//пишем тест с помощью функции добавления 0
        }
    }

    public string Zeropad(int number, int count)//функция добавления 0 (число; количество обязательных символов)
    {
        string answer = "";
        for (int i = count; i > 0; i = i - 1)//цикл с количества символов до 1
        {
            if (number / Mathf.Pow(10,i - 1) >= 1)//если число поделив на 10 в степени обязательных цифр больше 1, то возвращаем это число
            {
                return answer + number;
            }
            else
            {
                answer = answer + "0"; //иначе в ответ добавляем 0
            }
        }
        return answer;
    }


    

}


