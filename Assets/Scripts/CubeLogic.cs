using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLogic : MonoBehaviour
{
    public int xCount;
    public int yCount;
    public int zCount;
    public int[,,] Arr;
    public int zeroCoord;
    // Start is called before the first frame update
    void Start()
    {
        Arr = new int[xCount,yCount,zCount];
        for (int x=0; x < xCount; x++)
        {
            for ( int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < yCount; z++)
                {
                    Arr[x, y,z] = x * 100 + y*10+z;
                }
            }
        }
        Arr[0, 0,0] = 0;
        zeroCoord = 0;
    }

    public Vector3 Shift(int elementId)
    {
        int newX;
        int newY;
        int newZ;
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < yCount; z++)
                {
                    if (Arr[x, y, z] == elementId)
                    {
                        if (Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 10 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 1 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 100)
                        {
                            newX = zeroCoord/100;
                            newY = (zeroCoord / 10) % 10;
                            newZ = zeroCoord%10;
                            Arr[zeroCoord / 100, (zeroCoord / 10) % 10, zeroCoord % 10] = elementId;
                            Arr[x, y, z] = 0;
                            zeroCoord = x * 100 + y * 10 + z;
                            return new Vector3(newX-(xCount-1)*0.5f, newY - (yCount - 1) * 0.5f, newZ - (zCount - 1) * 0.5f);
                        }
                        else return new Vector3(x - (xCount - 1) * 0.5f, y - (yCount - 1) * 0.5f, z - (zCount - 1) * 0.5f);
                    }
                }
            }
        }
        return new Vector3(0,0,0);
    }
}
