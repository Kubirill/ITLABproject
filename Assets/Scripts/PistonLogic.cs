using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonLogic : MonoBehaviour
{
    public bool work = false;
    public GameObject level;
   

    private void OnMouseDown()
    {
        level.GetComponent<pistonLevel>().OperationPiston(gameObject.GetComponent<MovingObjectPistonLevel>().Xcoor, gameObject.GetComponent<MovingObjectPistonLevel>().Ycoor, work);
    }
    public void Close()
    {
        work = false;
        gameObject.GetComponent<Animator>().Play("close");
    }
    public void Open()
    {
        work = true;
        gameObject.GetComponent<Animator>().Play("open");

    }
}
