using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Menu: MonoBehaviour
{
    private int lastLevel = 1;
    private int openLevel=1;
    private int stage = 1;
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("levels"))
        {
            openLevel = PlayerPrefs.GetInt("levels");
            lastLevel = PlayerPrefs.GetInt("lastLevel");
            stage = PlayerPrefs.GetInt("stage");
            blocks = GameObject.FindGameObjectsWithTag("block");
            foreach (GameObject block in blocks)
            {
                if (block.name[5] - '0' <= openLevel ) GameObject.Destroy(block);
            }
        }

    }

    public void Save(int level,int stage)
    {
        PlayerPrefs.SetInt("lastLevel", level);
        PlayerPrefs.SetInt("stage",stage);
        if (lastLevel > openLevel)
        {
            openLevel = lastLevel;
            PlayerPrefs.SetInt("levels", openLevel);
        }
    }
    public void Save( int stage)
    {
        PlayerPrefs.SetInt("stage", stage);
    }

    public int GetStage()
    {
       return PlayerPrefs.GetInt("stage");
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt("levels");
    }

}
