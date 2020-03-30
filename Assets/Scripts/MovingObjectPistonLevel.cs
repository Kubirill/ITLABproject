using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectPistonLevel : MonoBehaviour
{
    public int Xcoor;
    public int Ycoor;

    public void ChangePosition(int newX,int newY, int maxX,int maxY)
    {
        Xcoor = newX;
        Ycoor = newY;
        transform.localPosition = new Vector3(-(Xcoor - (maxX - 1) * 0.5f), 1, (Ycoor - (maxY - 1) * 0.5f));

    }
}
