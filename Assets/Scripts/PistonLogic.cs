using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonLogic : MonoBehaviour
{
    public GameObject level;//уровень на котором находится поршень

    private int dir=0;//направление блока

    private void OnMouseDown()
    {
        level.GetComponent<pistonLevel>().OperationPiston(gameObject.GetComponent<MovingObjectPistonLevel>().Xcoor, gameObject.GetComponent<MovingObjectPistonLevel>().Ycoor);//активировать поршень
    }
    public void Close()//анимация закрытия блока
    {
        gameObject.GetComponent<Animator>().Play("close");
    }
    public void Open()//анимация открытия блока
    {
        //work = true;
        gameObject.GetComponent<Animator>().Play("open");
        dir = 0;

    }
    public void Open(int direction)//анимация открытия блока с направлением
    {
        
        //work = true;
        gameObject.GetComponent<Animator>().Play("open");
        dir = direction;//сменить направление
    }


    public void reversPiston()//придвинуть к себе блок
    {
        int xCoor = gameObject.GetComponent<MovingObjectPistonLevel>().Xcoor;//запоминаем координаты поршня
        int yCoor = gameObject.GetComponent<MovingObjectPistonLevel>().Ycoor;
        switch (dir)//в зависмости от направления
        {
            case 1:
                if ((level.GetComponent<pistonLevel>().Arr[xCoor + 2, yCoor] != '0') && (level.GetComponent<pistonLevel>().Arr[xCoor + 2, yCoor] != '8')) level.GetComponent<pistonLevel>().changePosition(xCoor + 2, yCoor, xCoor + 1, yCoor);//вызываем функцию сдвига(притягивания) если клетка перед поршнем пустая
                break;
            case 2:
                if ((level.GetComponent<pistonLevel>().Arr[xCoor, yCoor + 2] != '0') && (level.GetComponent<pistonLevel>().Arr[xCoor, yCoor + 2] != '8')) level.GetComponent<pistonLevel>().changePosition(xCoor, yCoor + 2, xCoor, yCoor + 1);
                break;
            case 3:
                if ((level.GetComponent<pistonLevel>().Arr[xCoor - 2, yCoor] != '0') && (level.GetComponent<pistonLevel>().Arr[xCoor - 2, yCoor] != '8')) level.GetComponent<pistonLevel>().changePosition(xCoor - 2, yCoor, xCoor - 1, yCoor);
                break;
            case 4:
                if ((level.GetComponent<pistonLevel>().Arr[xCoor, yCoor - 2] != '0') && (level.GetComponent<pistonLevel>().Arr[xCoor, yCoor - 2] != '8')) level.GetComponent<pistonLevel>().changePosition(xCoor, yCoor - 2, xCoor, yCoor - 1);
                break;
        }
    }
}
