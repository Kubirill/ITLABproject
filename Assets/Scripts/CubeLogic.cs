using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLogic : MonoBehaviour
{
    public int xCount;
    public int yCount;
    public int zCount;
    private int[,,] Arr;
    private int zeroCoord;
    public GameObject[] elements;
    private List<int> randVector = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        SetRandList();
        SetUp();
        RandSetUp();
     }

    private void SetRandList()
    {
        randVector.Add(100);
        randVector.Add(-100);
        randVector.Add(10);
        randVector.Add(-10);
        randVector.Add(1);
        randVector.Add(-1);
    }

    private void SetUp()
    {
        Arr = new int[xCount,yCount,zCount];
        zeroCoord = xCount*100  + yCount*10 + zCount - 222;
        for (int x=0; x < xCount; x++)
        {
            for ( int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    Arr[x, y,z] = x * 100 + y*10+z;
                    if (Arr[x, y, z] != zeroCoord)
                    {
                        string nameObj = returnName(Arr[x, y, z]);
                        foreach (GameObject el in elements)
                        {

                            if (el.name == nameObj)
                            {
                                Instantiate(el, new Vector3((x - (xCount - 1) * 0.5f), (y - (yCount - 1) * 0.5f), (z - (zCount - 1) * 0.5f)), Quaternion.identity, transform);
                                el.GetComponent<ElementLogic>().numElement = Arr[x, y, z];
                                el.GetComponent<ElementLogic>().parent = gameObject;
                            }
                        }
                    }
                }
            }
        }
        
    }
    

    private string returnName (int num)
    {
        int newX = num / 100;
        int newY = (num / 10) % 10;
        int newZ = num % 10;
        return returnSym(newX, xCount,1) + returnSym(newY, yCount,1) + returnSym(newZ, zCount,1);
    }

    private string returnSym(int num,int max,int or)
    {
        if (or == 1)
        {
            if (num == 0) return "0";
            if (num == max - 1) return "n";
            return "x";
        }
        else
        {
            if (num == 0) return "n";
            if (num == max - 1) return "0";
            return "x";
        }

    }

    private void RandSetUp()
    {
        for (int i = 0; i < xCount*yCount*zCount*10; i++)
        {


            int vr = randVector[Random.Range(0,6)];
            int pointX = zeroCoord / 100 + vr / 100;
            int pointY = (zeroCoord / 10) %10 + (vr / 10) % 10;
            int pointZ = zeroCoord % 10 + vr % 10;
            pointX = Mathf.Min(Mathf.Max(0, pointX), xCount-1);
            pointY = Mathf.Min(Mathf.Max(0, pointY), yCount-1);
            pointZ = Mathf.Min(Mathf.Max(0, pointZ), xCount-1);
            Vector3 trash = Shift(Arr[ pointX,pointY,pointZ]);
            
        }
        var elements = GameObject.FindGameObjectsWithTag("element");
        for (int i = 0; i < elements.Length; i++)
        {
            
            elements[i].GetComponent<ElementLogic>().SetPos();
        }
    }

    public Vector3 getStart(int elementId)
    {
        for (int x = 0; x < xCount; x++)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int z = 0; z < zCount; z++)
                {
                    if (Arr[x, y, z] == elementId) return new Vector3(x - (xCount - 1) * 0.5f, y - (yCount - 1) * 0.5f, z - (zCount - 1) * 0.5f);
                }
            }
        }
        return new Vector3(0,0,0);
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
                for (int z = 0; z < zCount; z++)
                {
                    if (Arr[x, y, z] == elementId)
                    {
                        
                        if (Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 10 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 1 || Mathf.Abs(zeroCoord - (x * 100 + y * 10 + z)) == 100)
                        {

                            newX = zeroCoord/100;
                            newY = (zeroCoord / 10) % 10;
                            newZ = zeroCoord%10;
                            Arr[newX, newY, newZ] = elementId;
                            Arr[x, y, z] = xCount*100+yCount*10+zCount-222;
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
