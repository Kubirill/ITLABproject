using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    float fpsTime = 0;
    private int nowTime;
    public GameObject cam;

    private void Awake()
    {
        nowTime = cam.GetComponent<Menu>().GetTime();
        gameObject.GetComponent<Text>().text = Zeropad(nowTime / 60, 2) + ":" + Zeropad(nowTime % 60, 2);
    }

    private void Update()
    {
        fpsTime = fpsTime + Time.deltaTime;
        if (fpsTime >= 1)
        {
            fpsTime = fpsTime - 1;
            nowTime++;cam.GetComponent<Menu>().NewTime(nowTime);
            gameObject.GetComponent<Text>().text = Zeropad(nowTime / 60, 2) + ":" + Zeropad(nowTime % 60, 2);
        }
    }

    public string Zeropad(int number, int count)
    {
        string answer = "";
        for (int i = count; i > 0; i = i - 1)
        {
            if (number / Mathf.Pow(10,i - 1) >= 1)
            {
                return answer + number;
            }
            else
            {
                answer = answer + "0";
            }
        }
        return answer;
    }


    public int Fact(int num)
    {
        if (num == 2) return 2;
        else return num * Fact(num - 1);
    }

}


