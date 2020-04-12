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
        int maxCountBlock =Mathf.Max( gameCube.GetComponent<CubeLogic>().xCount, gameCube.GetComponent<CubeLogic>().yCount, gameCube.GetComponent<CubeLogic>().zCount);
        gameCube.transform.localScale = new Vector3(0.95f / maxCountBlock, 0.95f / maxCountBlock, 0.95f / maxCountBlock);
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam.GetComponent<Menu>().GetStage() > 2) Win();
    }

    public string  getLevel(int lvl)
    {
        if (lvl == 1) return "558722880453100088040388448";
        if (lvl == 2) return "558822880203100088025387848";
        if (lvl == 3) return "558227815408800031040884888";
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

    public void OperationPiston(int xCoor, int yCoor)
    {
        
        
            if (Arr[xCoor, yCoor] =='1')
            {
                if (Arr[xCoor + 1, yCoor] != '0')
                {
                    
                    for (int i = xCoor + 1; i < xCount; i++)
                    {
                        if (Arr[i, yCoor] == '8') break;
                        if ((Arr[i, yCoor] == '0') || (Arr[i, yCoor] == '7'))
                    {
                            Shift(i, yCoor, 1, xCoor + 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor, 1);

            }

            if (Arr[xCoor, yCoor] =='2')
            {
                if (Arr[xCoor, yCoor + 1] != '0')
                {
                    for (int i = yCoor + 1; i < yCount; i++)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if ((Arr[xCoor, i] == '0') || (Arr[xCoor, i] == '7'))
                    {
                            Shift(xCoor, i,2, yCoor + 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor,2);
                
            }

            if (Arr[xCoor, yCoor] =='3')
            {
                if (Arr[xCoor - 1, yCoor] != '0')
                {
                    
                    for (int i = xCoor - 1; i >= 0; i--)
                    {
                        if (Arr[i, yCoor] == '8') break;
                        if ((Arr[i, yCoor] == '0') || (Arr[i, yCoor] == '7'))
                        {
                            Shift(i, yCoor, 3, xCoor - 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }
                }
                else changePiston(xCoor, yCoor,3);
                

            }

            if (Arr[xCoor, yCoor] =='4')
            {
                if (Arr[xCoor, yCoor - 1] != '0')
                {

                    for (int i = yCoor - 1; i >= 0; i--)
                    {
                        if (Arr[xCoor, i] == '8') break;
                        if ((Arr[xCoor, i] == '0')|| (Arr[xCoor, i] == '7'))
                        {
                            Shift(xCoor, i, 4, yCoor - 2);
                            
                            changePiston(xCoor, yCoor);
                            break;
                        }
                    }

                }
                else changePiston(xCoor, yCoor,4);
                
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
    public void changePosition(int startX,int startY,int endX,int endY)
    {
        char checkFinish = Arr[endX, endY];
        Arr[endX, endY] = Arr[startX, startY];
        foreach (GameObject mObj in movingObjects)
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor==startX) &&(mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == startY))
            {
                mObj.GetComponent<MovingObjectPistonLevel>().ChangePosition(endX, endY,xCount,yCount);
                break;
            }
            
        }
        
        Arr[startX, startY] = '0';
        if (checkFinish == '7')
        {
            if (Arr[endX, endY] == '5') Win();
            else Lose();
        }
    }
    private void changePiston(int xPiston, int yPiston)
    {
        foreach (GameObject mObj in movingObjects)
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor == xPiston) && (mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == yPiston))
            {
                mObj.GetComponent<PistonLogic>().Open();
                break;
            }

        }
    }
    private void changePiston(int xPiston, int yPiston,int direct)
    {
        foreach (GameObject mObj in movingObjects)
        {
            if ((mObj.GetComponent<MovingObjectPistonLevel>().Xcoor == xPiston) && (mObj.GetComponent<MovingObjectPistonLevel>().Ycoor == yPiston))
            {
                mObj.GetComponent<PistonLogic>().Open(direct);
                break;
            }

        }
    }

    private void Win()
    {
        gameCube.transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        gameObject.GetComponent<Rigidbody>().mass = 100;
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        cam.GetComponent<Menu>().Save(level-1, 3);
    }

    private void Lose()
    {
        Debug.Log("lose");
    }

    private bool findTarget;

    public void Update()
    {
        if ((transform.position.y < -5)&&(!findTarget))
        {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            cam.GetComponent<RotateCube>().newTarget(gameCube.transform);
            findTarget = true;
        }
        if (transform.position.y < -50) Destroy(gameObject);
    }
}
