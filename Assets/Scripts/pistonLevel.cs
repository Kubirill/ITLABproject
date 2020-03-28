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

    // ← ↑ → ↓1234 направление поршней
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
        if ((seedCode >=1) && (seedCode<=4)) Instantiate(piston,new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)),Quaternion.Euler(0,seedCode*90,0),parent);
        if (seedCode == 5) gameCube.transform.localPosition = new Vector3(-(x - (xCount - 1) * 0.5f),1,(y - (yCount - 1) * 0.5f));
        if (seedCode == 8) Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 1, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
        if (seedCode == 7) Instantiate(finish, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
        else
        {
            if ((x > 0) && (x < xCount - 1) && (y > 0) && (y < yCount - 1)) Instantiate(floor, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);
            else Instantiate(wall, new Vector3(-(x - (xCount - 1) * 0.5f), 0, (y - (yCount - 1) * 0.5f)), Quaternion.identity, parent);

        }
    }

}
