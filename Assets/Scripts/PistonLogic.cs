using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonLogic : MonoBehaviour
{
    public GameObject level;
   

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

    }
}
