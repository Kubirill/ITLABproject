using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistonLevel : MonoBehaviour
{
    public char[,] Arr;
    public GameObject floor;
    public GameObject finish;
    public GameObject wall;
    public GameObject piston;
    public GameObject gameCube;
    public int level=1;
    public Transform parent;

    private int xCount;
    private int yCount;
    private List<GameObject> movingObjects = new List<GameObject>();

    // → ↓← ↑ 1234 направление поршней
    //  → ↓← ↑qwer направление выдвенутых поршней
    //□ 0 - пустая клетка
    // F 7- финиш
    // X  5 Объект
    // ■ 8

    private void Start()
    {
        setLevel(level);
    }

    public string  getLevel(int lvl)
    {
        if (lvl == 1) return "558722880453100088040388448";
        else return "";
    }
    public void setLevel (int lvl)
    {
        string seed = getLevel(lvl);
        xCount =int.Parse( seed.Substring(0, 1));
        yCount = int.Parse(seed.Substring(1,1));
        seed = seed.Substring(2, seed.Length-2);
        Arr = new char[xCount, yCount];
        for (int y = 0; y < yCount; y++) 
        {
            for (int x = 0; x < xCount; x++)
            {
                Arr[x, y] = seed[y*xCount+x];
                spawnBox(Arr[x, y]-'0', x, y);
            }
            
        }
        
    }

    void spawnBox(int seedCode,int x, int y)
    {
        if ((seedCode >= 1) && (seedCode <= 4))
        {
            GameObject newObj = Instantiate(piston, new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)), Quaternion.Euler(0, seedCode * 90, 0), parent);
            newObj.GetComponent<MovingObjectPistonLevel>().Xcoor = x;
            newObj.GetComponent<MovingObjectPistonLevel>().Ycoor = y;
            newObj.GetComponent<PistonLogic>().level = gameObject;
            movingObjects.Add(newObj);
        }
        if (seedCode == 5)
        {
            gameCube.transform.localPosition = new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f));
            gameCube.GetComponent<MovingObjectPistonLevel>().Xcoor = x;
            gameCube.GetComponent<MovingObjectPistonLevel>().Ycoor = y;
            movingObjects.Add(gameCube);
        }
        if (seedCode == 8) Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
        if (seedCode == 7) Instantiate(finish, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
        else
        {
            if ((x > 0) && (x < xCount - 1) && (y > 0) && (y < yCount - 1))
            {
                Instantiate(floor, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
                
            }
            else
            {
                Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
            }
        }
    }

    public void OperationPiston(int xCoor, int yCoor, bool worked)
    {
        if (!worked)
        {
            if (Arr[xCoor, yCoor] =='1')
            {
                if (Arr[xCoor + 1, yCoor] != 0)
                {
                    for (int i = xCoor + 1; i < xCount; i++)
                    {
                        if (Arr[i, yCoor] == '8') break;
                        if (Arr[i, yCoor] == '0')
                        {
                            Shift(i, yCoor, 1, xCoor + 2);
                            Arr[xCoor, yCoor] = 'q';
                            changePiston(xCoor, yCoor, true);
                            break;
                        }
                    }
                }
                else
                {
                    changePiston(xCoor, yCoor, true);
                    closePistonesAround(xCoor+1, yCoor);
                }

            }

            if (Arr[xCoor, yCoor] =='2')
            {
                if (Arr[xCoor, yCoor + 1] != 0)
                {
                    for (int i = yCoor + 1; i < yCount; i++)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if (Arr[xCoor, i] == '0')
                        {
                            Shift(xCoor, i,2, yCoor + 2);
                            Arr[xCoor, yCoor] = 'w';
                            changePiston(xCoor, yCoor,true);
                            break;
                        }
                    }
                }
                else
                {
                    changePiston(xCoor, yCoor, true);
                    closePistonesAround(xCoor , yCoor+1);
                }


            }

            if (Arr[xCoor, yCoor] =='3')
            {
                if (Arr[xCoor - 1, yCoor] != 0)
                {
                    for (int i = xCoor - 1; i >= 0; i--)
                    {
                        if (Arr[i, yCoor] == '8') break;
                        if (Arr[i, yCoor] == '0')
                        {
                            Shift(i, yCoor, 3, xCoor - 2);
                            Arr[xCoor, yCoor] = 'e';
                            changePiston(xCoor, yCoor,true);
                            break;
                        }
                    }
                }
                else
                {
                    changePiston(xCoor, yCoor, true);
                    closePistonesAround(xCoor-1 , yCoor);
                }

            }

            if (Arr[xCoor, yCoor] =='4')
            {
                if (Arr[xCoor, yCoor - 1] != 0)
                {

                    for (int i = yCoor - 1; i >= 0; i--)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if (Arr[xCoor, i] == '0')
                        {
                            Shift(xCoor, i, 4, yCoor - 2);
                            Arr[xCoor, yCoor] = 'r';
                            changePiston(xCoor, yCoor,true);
                            break;
                        }
                    }

                }
                else
                {
                    changePiston(xCoor, yCoor, true);
                    closePistonesAround(xCoor , yCoor-1);
                }

            }
        }
        else
        {
            if (Arr[xCoor, yCoor] == 'q')
            {
                if ((Arr[xCoor + 2, yCoor] != 0)&& (Arr[xCoor + 2, yCoor] != 8))
                {
                    changePosition(xCoor + 2, yCoor, xCoor + 1, yCoor);

                }
                changePiston(xCoor, yCoor,false);
                Arr[xCoor, yCoor] = '1';
            }
            if (Arr[xCoor, yCoor] == 'w')
            {
                if ((Arr[xCoor , yCoor+ 2] != 0) && (Arr[xCoor , yCoor+ 2] != 8))
                {
                    changePosition(xCoor, yCoor + 2, xCoor , yCoor+ 1);

                }
                changePiston(xCoor, yCoor, false);
                Arr[xCoor, yCoor] = '2';
            }
            if (Arr[xCoor, yCoor] == 'e')
            {
                if ((Arr[xCoor - 2, yCoor] != 0) && (Arr[xCoor - 2, yCoor] != 8))
                {
                    changePosition(xCoor - 2, yCoor, xCoor - 1, yCoor);

                }
                changePiston(xCoor, yCoor, false);
                Arr[xCoor, yCoor] = '3';
            }
            if (Arr[xCoor, yCoor] == 'r')
            {
                if ((Arr[xCoor , yCoor-2] != 0) && (Arr[xCoor, yCoor-2] != 8))
                {
                    changePosition(xCoor , yCoor-2, xCoor , yCoor-1);

                }
                changePiston(xCoor, yCoor, false);
                Arr[xCoor, yCoor] = '4';
            }

        }

    }
    private void Shift (int xCoor, int yCoor, int dir, int coorEnd)
    {
        if (dir == 1)
        {
            for (int i = xCoor; i>= coorEnd; i--)
            {
                changePosition(i - 1, yCoor,i, yCoor);/////!!!!
            }
        }

        if (dir == 2)
        {
            for (int i = yCoor; i >= coorEnd; i--)
            {
                changePosition(xCoor,i - 1, xCoor, i);
            }
        }

        if (dir == 3)
        {
            for (int i = xCoor; i <= coorEnd; i++)
            {
                changePosition(i + 1, yCoor, i, yCoor);
            }
        }

        if (dir == 4)
        {
            for (int i = yCoor; i <= coorEnd; i++)
            {
                changePosition(xCoor,i + 1, xCoor, i );
            }
        }
    }
    private void changePosition(int startX,int startY,int endX,int endY)
    {
        Arr[endX, endY] = Arr[startX, startY];
        foreach (GameObject mObj in movingObjects)
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor==startX) &&(mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == startY))
            {
                mObj.GetComponent<MovingObjectPistonLevel>().ChangePosition(endX, endY,xCount,yCount);
                if ((Arr[endX, endY]=='q') || (Arr[endX, endY] == 'w')||(Arr[endX, endY] == 'e')||(Arr[endX, endY] == 'r')) mObj.GetComponent<PistonLogic>().Close();
                break;
            }
            
        }
        switch (Arr[endX, endY])
            {
                case 'q':
                    Arr[endX, endY] = '1';
                    break;
                case 'w':
                    Arr[endX, endY] = '2';
                    break;
                case 'e':
                    Arr[endX, endY] = '3';
                    break;
                case 'r':
                    Arr[endX, endY] = '4';
                    break;
                default:
                    break;
            }
        Arr[startX, startY] = '0';
    }
    private void changePiston(int xPiston, int yPiston,bool open)
    {
        foreach (GameObject mObj in movingObjects)
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor == xPiston) && (mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == yPiston))
            {
                if (open) mObj.GetComponent<PistonLogic>().Open();
                else mObj.GetComponent<PistonLogic>().Close();
                break;
            }

        }
    }
    private void closePistonesAround(int xCoor,int yCoor)
    {
        for (int y = 0; y < yCount; y++)
        {
            Debug.Log(Arr[0,y]+""+ Arr[1, y] + "" + Arr[2, y] + "" + Arr[3, y] + "" + Arr[4, y] + "");
            
        }
        if (Arr[xCoor, yCoor - 1] == 'w')
        {
            Arr[xCoor, yCoor - 1] = '2';
            changePiston(xCoor, yCoor - 1, false);
        }
        if (Arr[xCoor+1, yCoor ] == 'e')
        {
            Arr[xCoor + 1, yCoor] = '3';
            changePiston(xCoor+1, yCoor , false);
        }
        if (Arr[xCoor, yCoor + 1] == 'r')
        {
            Arr[xCoor, yCoor + 1] = '4';
            changePiston(xCoor, yCoor + 1, false);
        }
        if (Arr[xCoor-1, yCoor] == 'q')
        {
            Arr[xCoor - 1, yCoor] = '1';
            changePiston(xCoor - 1, yCoor, false);
        }
    }
}
