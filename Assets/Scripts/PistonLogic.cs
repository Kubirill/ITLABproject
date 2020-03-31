using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonLogic : MonoBehaviour
{
    public GameObject level;

    private int dir=0;

    private void OnMouseDown()
    {
        level.GetComponent<pistonLevel>().OperationPiston(gameObject.GetComponent<MovingObjectPistonLevel>().Xcoor, gameObject.GetComponent<MovingObjectPistonLevel>().Ycoor);
    }
    public void Close()
    {
        gameObject.GetComponent<Animator>().Play("close");
    }
    public void Open()
    {
        //work = true;
        gameObject.GetComponent<Animator>().Play("open");
        dir = 0;

    }
    public void Open(int direction)
    {
        //work = true;
        gameObject.GetComponent<Animator>().Play("open");
        dir = direction;
    }


    public void reversPiston()
    {
        int xCoor = gameObject.GetComponent<MovingObjectPistonLevel>().Xcoor;
        int yCoor = gameObject.GetComponent<MovingObjectPistonLevel>().Ycoor;
        switch (dir)
        {
            case 1:
                if ((level.GetComponent<pistonLevel>().Arr[xCoor + 2, yCoor] != '0') && (level.GetComponent<pistonLevel>().Arr[xCoor + 2, yCoor] != '8')) level.GetComponent<pistonLevel>().changePosition(xCoor + 2, yCoor, xCoor + 1, yCoor);
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
